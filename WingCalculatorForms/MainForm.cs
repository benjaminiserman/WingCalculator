namespace WingCalculatorForms;

using System;
using System.Text;
using System.Windows.Forms;
using WingCalculatorForms.Properties;
using WingCalculatorShared;

public partial class MainForm : Form
{
	private Solver _solver;

	private bool _skipSelect = false;
	private readonly StringBuilder _stdout = new();
	private bool _darkMode = false;
	public static readonly string emptyEntry = "\n\n";
	private int _textIndex;
	private int _historyIndex = 0;

	public MainForm()
	{
		InitializeComponent();

		historyView.Items.Clear();
		historyView.Items.Add(emptyEntry);

		KeyPreview = true;
		KeyPress += new KeyPressEventHandler(FormControlKeys);

		historyView.PreviewKeyDown += new PreviewKeyDownEventHandler(HandleDeleteKey);

		omnibox.KeyUp += new KeyEventHandler(OmniboxControlKeys);

		RefillBuffer();
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

	private void omnibox_TextChanged(object sender, EventArgs e) // remove leading \r\n
	{
		if (omnibox.Text.Length >= 2 && omnibox.Text[0..2] == "\r\n") omnibox.Text = omnibox.Text[2..];
	}

	private void OmniboxControlKeys(object send, KeyEventArgs e)
	{
		switch (e.KeyCode)
		{
			case Keys.Escape:
			{
				historyView.Select(); // move focus back to MainForm
				break;
			}
			case Keys.Up:
			{
				if (omnibox.SelectionStart != _textIndex)
				{
					break;
				}
				else if (historyView.SelectedIndex == -1)
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
				if (omnibox.SelectionStart != _textIndex)
				{
					break;
				}
				else if (historyView.SelectedIndex == -1)
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
					HandleDeleteKey(send, new PreviewKeyDownEventArgs(e.KeyCode));
				}

				break;
			}
		}

		_textIndex = omnibox.SelectionStart;
	}

	private void FormControlKeys(object sender, KeyPressEventArgs e)
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

	private void Execute()
	{
		bool altMode = false; // if true, *do not* modify current entry, even if selected

		if (string.IsNullOrWhiteSpace(omnibox.Text))
		{
			if (historyView.Items.Count < 2 || string.IsNullOrWhiteSpace((string)historyView.Items[^2])) return;
			else omnibox.Text = GetEntryText((string)historyView.Items[^2]);
		}
		else if ((ModifierKeys & Keys.Shift) != 0 && historyView.SelectedItem != null) altMode = true;

		_historyIndex = -1;

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
			_textIndex = omnibox.SelectionStart;
			if (historyView.SelectedItem != null && !altMode) historyView.Items[historyView.SelectedIndex] = omnibox.Text;
			return;
		}

		if (historyView.SelectedItem != null && !altMode)
		{
			historyView.Items[historyView.SelectedIndex] = solveString;
			RecalculateEntries(historyView.SelectedIndex + 1);
		}
		else
		{
			historyView.Items.Add(solveString);
		}

		_textIndex = 0;
		_skipSelect = true;
		historyView.SelectedIndex = 0;
		historyView.SelectedIndex = -1;
		historyView.TopIndex = historyView.Items.Count - 1;
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

	private void HistoryViewIndexChanged(object sender, EventArgs e)
	{
		if (_skipSelect) _skipSelect = false;
		else if (historyView.SelectedItem is not null)
		{
			if (_historyIndex != -1)
			{
				if (GetEntryText(historyView.Items[_historyIndex]) != omnibox.Text)
				{
					historyView.Items[_historyIndex] = omnibox.Text;
				}
			}

			omnibox.Text = GetEntryText(historyView.SelectedItem);
		}

		if (omnibox.Text.Length > 0) // send cursor to far right
		{
			omnibox.SelectionStart = omnibox.Text.Length;
			_textIndex = omnibox.SelectionStart;
		}

		_historyIndex = historyView.SelectedIndex;

		RefillBuffer();
	}

	private void RefillBuffer()
	{
		for (int i = 0; i < historyView.Items.Count - 1; i++) // remove empty buffer entries that aren't at the end
		{
			if (string.IsNullOrWhiteSpace((string)historyView.Items[i]))
			{
				historyView.Items.RemoveAt(i);
				i--;
			}
		}

		if ((string)historyView.Items[^1] != emptyEntry) historyView.Items.Add(emptyEntry); // add empty buffer entry
	}

#pragma warning restore IDE1006

	private static string GetEntryText(object x) => ((string)x).Split('\n')[0];

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
