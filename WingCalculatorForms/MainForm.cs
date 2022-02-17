namespace WingCalculatorForms;

using System;
using System.Text;
using System.Windows.Forms;
using WingCalculatorForms.Properties;
using WingCalculatorShared;
using WingCalculatorShared.Exceptions;

public partial class MainForm : Form
{
	private Solver _solver;
	public ViewerForm ViewerForm { get; } = new();

	private readonly StringBuilder _stdout = new();
	private bool _darkMode = false;
	private int _textIndex;
	private float _currentFontSize = 9;

	public MainForm()
	{
		InitializeComponent();

		historyView.Connect(this);
		historyView.Clear();

		KeyPreview = true;
		KeyPress += new KeyPressEventHandler(FormControlKeys);

		historyView.PreviewKeyDown += new PreviewKeyDownEventHandler(HandleDeleteKey);

		omnibox.KeyUp += new KeyEventHandler(OmniboxControlKeys);

		ResetSolver();
	}

#pragma warning disable IDE1006 // Naming Styles
	#region CalculatorButtons
	private void bin_button_Click(object sender, EventArgs e) => SendString(":2");

	private void pi_button_Click(object sender, EventArgs e) => SendString("$PI");

	private void clr_button_Click(object sender, EventArgs e) => omnibox.Clear();

	private void exe_button_Click(object sender, EventArgs e) => Execute();

	private void hex_button_Click(object sender, EventArgs e) => SendString(":16");

	private void tau_button_Click(object sender, EventArgs e) => SendString("$TAU");

	private void ans_button_Click(object sender, EventArgs e) => SendString("$ANS");

	private void ac_button_Click(object sender, EventArgs e)
	{
		ResetSolver();
		historyView.Clear();
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

	private void sin_button_Click(object sender, EventArgs e) => SendString("sin");

	private void cos_button_Click(object sender, EventArgs e) => SendString("cos");

	private void tan_button_Click(object sender, EventArgs e) => SendString("tan");

	private void pow_button_Click(object sender, EventArgs e) => SendString("**");

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

	private void viewerButton_Click(object sender, EventArgs e)
	{
		if (!ViewerForm.Visible)
		{
			ViewerForm.Show();
		}
	}

	#endregion

	private void omnibox_TextChanged(object sender, EventArgs e) // remove leading \r\n
	{
		if (omnibox.Text.Length >= 2 && omnibox.Text[0..2] == "\r\n") omnibox.Text = omnibox.Text[2..];
	}

	private void OmniboxControlKeys(object send, KeyEventArgs e) // capture CTRL +/-, ESC, DEL, arrow keys
	{
		if (ModifierKeys.HasFlag(Keys.Control))
		{
			if (e.KeyCode == Keys.Oemplus)
			{
				_currentFontSize++;
				FontSizer.ApplySize(Controls, this, _currentFontSize);
				e.Handled = true;
				return;
			}
			else if (e.KeyCode == Keys.OemMinus)
			{
				_currentFontSize--;
				FontSizer.ApplySize(Controls, this, _currentFontSize);
				e.Handled = true;
				return;
			}
		}

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
				else
				{
					omnibox.Text = historyView.SelectedUp(omnibox.Text);
					SendCursorRight();
				}

				break;
			}
			case Keys.Down:
			{
				if (omnibox.SelectionStart != _textIndex)
				{
					break;
				}
				else
				{
					omnibox.Text = historyView.SelectedDown(omnibox.Text);
					SendCursorRight();
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

	private void FormControlKeys(object sender, KeyPressEventArgs e) // focus keys to omnibox, capture return
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
			omnibox.Text = historyView.DeleteSelected();
		}
	}

	private void Execute()
	{
		bool altMode = false; // if true, *do not* modify current entry, even if selected

		if (string.IsNullOrWhiteSpace(omnibox.Text))
		{
			if (historyView.Items.Count < 2) return;
			else
			{
				omnibox.Text = historyView.GetLastNonEmptyEntry();  // for duplicating last entry if omnibox empty
				if (string.IsNullOrWhiteSpace(omnibox.Text)) return;
			}
		}
		else if ((ModifierKeys & Keys.Shift) != 0 && historyView.SelectedItem != null) altMode = true;

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
			historyView.EditSelected(solveString);
			CalculateEntries(historyView.SelectedIndex + 1);
		}
		else
		{
			historyView.AddEntry(solveString);
		}

		_textIndex = 0;
		ViewerForm.RefreshEntries(_solver);
		SendCursorRight();
		historyView.SelectedClear(); // yes, this double set is necessary... it triggers an event iirc
		historyView.TopIndex = historyView.Items.Count - 1; // send scrollbar to bottom
		omnibox.Clear();
	}

	#region Recalculate
	private void CalculateEntries(int start)
	{
		for (int i = start; i < historyView.Items.Count; i++)
		{
			Calculate(i);
		}
	}

	private void Calculate(int i)
	{
		try
		{
			if (!string.IsNullOrWhiteSpace((string)historyView.Items[i]))
			{
				historyView.EditAt(i, GetSolve(historyView.GetEntryText(i)));
			}
		}
		catch (WingCalcException ex)
		{
			historyView.Items[i] = $"{historyView.Items[i]}\n{ex.Message}";
		}
		catch (Exception ex)
		{
			historyView.Items[i] = $"{historyView.Items[i]}\n{ex.GetType()}: {ex.Message}";
		}
	}
	#endregion

	private void SendCursorRight()
	{
		if (omnibox.Text.Length > 0)
		{
			omnibox.SelectionStart = omnibox.Text.Length;
			_textIndex = omnibox.SelectionStart;
		}
	}

	private void HistoryViewIndexChanged(object sender, EventArgs e)
	{
		return;
		/*
		if (historyView.SelectedItem is not null)
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

		RefillBuffer();*/
	}

	public string OmniText { get => omnibox.Text; set => omnibox.Text = value; }

	private string GetSolve(string s)
	{
		try
		{
			double solve = _solver.Solve(s);

			string _stdoutGot = _stdout.ToString();
			_stdout.Clear();

			return $"{s}\n{_stdoutGot}> Solution: {solve}";

		}
		catch (WingCalcException ex)
		{
			_stdout.Clear();
			string errorMessage = ex.Message.Replace("&", "&&");
			errorLabel.Text = errorMessage;
			throw;
		}
		catch (Exception ex)
		{
			_stdout.Clear();

			string errorMessage = $"{ex.GetType()}: {ex.Message}".Replace("&", "&&");

#if DEBUG
			errorMessage = $"{errorLabel.Text} @ {ex.StackTrace.Replace("&", "&&")}";
#endif

			errorLabel.Text = errorMessage;
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

		ViewerForm.RefreshEntries(_solver);
	}

	#region IOHooks

	private void WriteError(string s) => errorLabel.Text = s;
	private void WriteLine(string s) => _stdout.AppendLine(s);
	private void Write(string s) => _stdout.Append(s);
	private static string ReadLine() => throw new PlatformNotSupportedException("Prompting is not available in the WinForms app.");

	#endregion

	private void errorLabel_Click(object sender, EventArgs e)
	{
		string errorText = errorLabel.Text.Replace("&&", "&");
		string copyText = "Copied to clipboard.";

		if (string.IsNullOrWhiteSpace(errorText) || errorLabel.Text == copyText) return;

		Clipboard.SetText(errorText);
		errorLabel.Text = copyText;

		Timer t = new();
		t.Interval = 1000;
		t.Tick += (_, _) => RefillErrorLabel(t, errorLabel.Text, errorText);
		t.Start();
	}

	private void RefillErrorLabel(Timer t, string compare, string set)
	{
		if (errorLabel.Text == compare) errorLabel.Text = set.Replace("&", "&&");
		t.Enabled = false;
	}

#pragma warning restore IDE1006 // $$$ Bad, move this
}
