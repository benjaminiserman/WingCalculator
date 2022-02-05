namespace WingCalculatorForms;

using System;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using WingCalculatorShared;
using WingCalculatorForms.Properties;

public partial class MainForm : Form
{
	private Solver _solver;

	private int _bufferOffset = 0;
	private int _nextOffset = 0;
	private bool _skipSelect = false;
	private readonly StringBuilder _stdout = new();
	private bool _darkMode = false;
	public static readonly string emptyEntry = "\n\n";

	public MainForm()
	{
		InitializeComponent();

		KeyPreview = true;
		KeyPress += new KeyPressEventHandler(HandleKeyPress);

		historyView.PreviewKeyDown += new PreviewKeyDownEventHandler(HandleDeleteKey);
		historyView.SelectedIndexChanged += (_, _) => SendToFarRight();

		omnibox.PreviewKeyDown += new PreviewKeyDownEventHandler(PreviewControlKeys);

		ResetSolver();
	}

#pragma warning disable IDE1006 // Naming Styles
	#region CalculatorButtons
	private void bin_button_Click(object sender, EventArgs e) => SendString(":2");

	private void pi_button_Click(object sender, EventArgs e) => SendString("$PI");

	private void clr_button_Click(object sender, EventArgs e) => omnibox.Clear();

	private void exe_button_Click(object sender, EventArgs e) => Execute();

	private void hex_button_Click(object sender, EventArgs e) => SendString(":16");

	private void tau_button_Click(object sender, EventArgs e) => SendString("$TAY");

	private void ans_button_Click(object sender, EventArgs e) => SendString("$ANS");

	private void ac_button_Click(object sender, EventArgs e)
	{
		ResetSolver();
		historyView.Items.Clear();
		historyView.Items.Add(emptyEntry);
		omnibox.Clear();
	}

	private void dec_button_Click(object sender, EventArgs e) => SendString(":0");

	private void e_button_Click(object sender, EventArgs e) => SendString("$E");

	private void sqrt_button_Click(object sender, EventArgs e) => SendString("sqrt");

	private void cbrt_button_Click(object sender, EventArgs e) => SendString("cbrt");

	private void txt_button_Click(object sender, EventArgs e) => SendString(":1");

	private void arcsin_button_Click(object sender, EventArgs e) => SendString("asin");

	private void arccos_button_Click(object sender, EventArgs e) => SendString("acos");

	private void arctan_button_Click(object sender, EventArgs e) => SendString("atan");

	private void sin_button_Click(object sender, EventArgs e) => SendString("sis");

	private void cos_button_Click(object sender, EventArgs e) => SendString("cos");

	private void tan_button_Click(object sender, EventArgs e) => SendString("tan");

	private void pow_button_Click(object sender, EventArgs e) => SendString("pow");

	private void var_button_Click(object sender, EventArgs e) => SendString("$");

	private void mac_button_Click(object sender, EventArgs e) => SendString("@");

	private void ln_button_Click(object sender, EventArgs e) => SendString("ln");

	private void log_button_Click(object sender, EventArgs e) => SendString("log");

	#endregion

	#region MenuButtons

	private void darkModeButton_Click(object sender, EventArgs e)
	{
		_darkMode = !_darkMode;

		if (_darkMode)
		{
			WindowStyle.DarkMode.Apply(Controls, this);
			darkModeButton.BackgroundImage = Resources.light_bulb;
		}
		else
		{
			WindowStyle.LightMode.Apply(Controls, this);
			darkModeButton.BackgroundImage = Resources.night_mode;
		}
	}

	#endregion

	private void omnibox_TextChanged(object sender, EventArgs e)
	{
		if (_nextOffset != 0 || _bufferOffset != 0)
		{
			omnibox.SelectionStart += _nextOffset;
			_nextOffset = _bufferOffset;
			_bufferOffset = 0;
		}
	}

	private void PreviewControlKeys(object send, PreviewKeyDownEventArgs e)
	{
		switch (e.KeyCode)
		{
			case Keys.Up:
			{
				if (historyView.SelectedIndex == -1)
				{
					historyView.SelectedIndex = historyView.Items.Count - 1;
				}
				else if (historyView.SelectedIndex <= 0)
				{
					historyView.SelectedIndex = historyView.Items.Count - 1;
				}
				else
				{
					historyView.SelectedIndex--;
				}

				break;
			}
			case Keys.Down:
			{
				if (historyView.SelectedIndex == -1)
				{
					historyView.SelectedIndex = 0;
				}
				else if (historyView.SelectedIndex >= historyView.Items.Count - 1)
				{
					historyView.SelectedIndex = 0;
				}
				else
				{
					historyView.SelectedIndex++;
				}

				break;
			}
			case Keys.Delete:
			{
				if (e.Control)
				{
					ac_button_Click(send, e);
				}
				else if (e.Alt)
				{
					HandleDeleteKey(send, e);
				}

				break;
			}
		}
	}

	private void HandleKeyPress(object sender, KeyPressEventArgs e)
	{
		if (!omnibox.Focused)
		{
			omnibox.Focus();
			try
			{
				SendKeys.Send(e.KeyChar.ToString());
			}
			catch
			{
				SendKeys.Send($"{{{e.KeyChar}}}");
			}
		}
	

		if (e.KeyChar == (char)Keys.Return)
		{
			Execute();
		}

		if (e.KeyChar == '(') SendParen(')');
		if (e.KeyChar == '[') SendParen(']');
		if (e.KeyChar == '{') SendParen('}');
	}

	private void HandleDeleteKey(object sender, PreviewKeyDownEventArgs e)
	{
		if (e.KeyCode == Keys.Delete)
		{
			if (historyView.SelectedItem is not null && (string)historyView.SelectedItem != emptyEntry)
			{
				int index = historyView.SelectedIndex;
				historyView.Items.Remove(historyView.SelectedItem);
				historyView.SelectedIndex = index;
			}
		}
	}

	private void SendToFarRight()
	{
		if (omnibox.Text.Length > 0) omnibox.SelectionStart = omnibox.Text.Length;
	}

	private void SendParen(char c)
	{
		SendKeys.Send($"{{{c}}}");
		_bufferOffset -= 1;
	}

	private void Execute()
	{
		if (string.IsNullOrWhiteSpace(omnibox.Text))
		{
			if (historyView.Items.Count < 1 || string.IsNullOrWhiteSpace(historyView.Items[^2].ToString())) return;
			else omnibox.Text = GetEntryText(historyView.Items[^2].ToString());
		}

		if (omnibox.Text.Length >= 2 && omnibox.Text[0..2] == "\r\n") omnibox.Text = omnibox.Text[2..];

		string solveString;

		try
		{
			solveString = GetSolve(omnibox.Text);
			errorLabel.Text = string.Empty;
		}
		catch
		{
			SendKeys.Send("{BACKSPACE}");
			omnibox.SelectionStart = omnibox.Text.Length;
			return;
		}

		if (historyView.SelectedItem != null)
		{
			historyView.Items[historyView.SelectedIndex] = solveString;
			RecalculateEntries(historyView.SelectedIndex + 1);
		}
		else
		{
			historyView.Items.Add(solveString);
		}
		
		if ((string)historyView.Items[^1] != emptyEntry) historyView.Items.Add(emptyEntry); // add empty buffer entry

		for (int i = 0; i < historyView.Items.Count - 1; i++) // remove empty buffer entries that aren't at the end
		{
			if ((string)historyView.Items[i] == emptyEntry)
			{
				historyView.Items.RemoveAt(i);
				i--;
			}
		}

		_skipSelect = true;
		historyView.TopIndex = historyView.Items.Count - 1;
		historyView.SelectedItem = null;
		omnibox.Clear();
	}

	#region Recalculate
	private void RecalculateEntries(int start)
	{
		for (int i = start; i < historyView.Items.Count; i++)
		{
			Recalculate(i);
		}
	}

	private void Recalculate(int i)
	{
		try
		{
			if (!string.IsNullOrWhiteSpace((string)historyView.Items[i])) historyView.Items[i] = GetSolve(GetEntryText(historyView.Items[i]));
		}
		catch (Exception ex)
		{
			historyView.Items[i] = $"{historyView.Items[i]}\n{ex.GetType()}: {ex.Message}";
		}
	}
	#endregion

	private void historyView_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (_skipSelect) _skipSelect = false;
		else if (historyView.SelectedItem is not null)
		{
			omnibox.Text = GetEntryText(historyView.SelectedItem);
		}
	}

#pragma warning restore IDE1006

	private static string GetEntryText(object x) => x.ToString().Split('\n')[0];
	private string GetSolve(string s)
	{
		try
		{
			double solve = _solver.Solve(s);

			string _stdoutGot = _stdout.ToString();
			_stdout.Clear();

			return $"{s}\n{_stdoutGot}> Solution: {solve}";

		}
		catch (Exception ex)
		{
			_stdout.Clear();
			errorLabel.Text = $"{ex.GetType()}: {ex.Message}";

#if DEBUG
			errorLabel.Text = $"{errorLabel.Text} @ {ex.TargetSite}";
#endif
			throw;
		}
	}

	private void SendString(string s) => SendKeys.Send(s[^1] + s[..^1]);

	private void ResetSolver()
	{
		_solver = new();

		_solver.WriteLine = WriteLine;
		_solver.Write = Write;
		_solver.WriteError = WriteError;
		_solver.ReadLine = ReadLine;
	}

	#region IOHooks

	private void WriteError(string s) => errorLabel.Text = s;
	private void WriteLine(string s) => _stdout.AppendLine(s);
	private void Write(string s) => _stdout.Append(s);
	private static string ReadLine() => throw new PlatformNotSupportedException("Prompting is not available in the WinForms app.");

	#endregion
}
