namespace WingCalculatorForms;
using System;
using System.Text;
using System.Windows.Forms;
using WingCalculatorForms.Properties;
using WingCalculatorShared;

public partial class MainForm : Form
{
	public Solver Solver { get; private set; }
	public ViewerForm ViewerForm { get; } = new();

	public StringBuilder Stdout { get; private set; } = new();
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

		omnibox.KeyUp += new KeyEventHandler(OmniboxControlKeys);
		ResizeEnd += (_, _) => historyView.RefreshEntries();

		ResetSolver();
	}

#pragma warning disable IDE1006 // Naming Styles
	#region CalculatorButtons
	private void bin_button_Click(object sender, EventArgs e) => SendString("::$BIN");

	private void pi_button_Click(object sender, EventArgs e) => SendString("$PI");

	private void clr_button_Click(object sender, EventArgs e)
	{
		omnibox.Clear();
		errorLabel.Text = string.Empty;
	}

	private void exe_button_Click(object sender, EventArgs e) => Execute();

	private void hex_button_Click(object sender, EventArgs e) => SendString("::$HEX");

	private void tau_button_Click(object sender, EventArgs e) => SendString("$TAU");

	private void ans_button_Click(object sender, EventArgs e) => SendString("$ANS");

	private void ac_button_Click(object sender, EventArgs e)
	{
		ResetSolver();
		historyView.Clear();
		omnibox.Clear();
		errorLabel.Text = string.Empty;
	}

	private void frac_button_Click(object sender, EventArgs e) => SendString("::$FRAC");

	private void e_button_Click(object sender, EventArgs e) => SendString("$E");

	private void sqrt_button_Click(object sender, EventArgs e) => SendString("sqrt", true);

	private void cbrt_button_Click(object sender, EventArgs e) => SendString("cbrt", true);

	private void txt_button_Click(object sender, EventArgs e) => SendString("::$TXT");

	private void arcsin_button_Click(object sender, EventArgs e) => SendString("asin", true);

	private void arccos_button_Click(object sender, EventArgs e) => SendString("acos", true);

	private void arctan_button_Click(object sender, EventArgs e) => SendString("atan", true);

	private void sin_button_Click(object sender, EventArgs e) => SendString("sin", true);

	private void cos_button_Click(object sender, EventArgs e) => SendString("cos", true);

	private void tan_button_Click(object sender, EventArgs e) => SendString("tan", true);

	private void pow_button_Click(object sender, EventArgs e) => SendString("**");

	private void var_button_Click(object sender, EventArgs e) => SendString("$");

	private void mac_button_Click(object sender, EventArgs e) => SendString("@");

	private void ln_button_Click(object sender, EventArgs e) => SendString("ln", true);

	private void log_button_Click(object sender, EventArgs e) => SendString("log", true);

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

		ViewerForm.Select();
	}

	#endregion

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

		if (e.KeyCode == Keys.PageUp) omnibox.Text = historyView.SelectedChange(0, omnibox.Text);
		if (e.KeyCode == Keys.PageDown) omnibox.Text = historyView.SelectedChange(historyView.Items.Count - 1, omnibox.Text);

		switch (e.KeyCode)
		{
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
		}

		_textIndex = omnibox.SelectionStart;
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return && e.Alt)
		{
			Execute();
			e.Handled = true;
			e.SuppressKeyPress = true;
		}
		else if (e.KeyCode == Keys.Back && e.Alt)
		{
			omnibox.Text = historyView.DeleteSelected();
			e.Handled = true;
			e.SuppressKeyPress = true;
		}
		else if (e.KeyCode == Keys.Delete && e.Alt)
		{
			ac_button_Click(this, e);
			e.Handled = true;
			e.SuppressKeyPress = true;
		}
		else base.OnKeyDown(e);
	}

	private void FormControlKeys(object sender, KeyPressEventArgs e) // focus keys to omnibox, capture return
	{
		if (e.KeyChar == (char)Keys.Return && !ModifierKeys.HasFlag(Keys.Shift))
		{
			Execute();
		}

		if (!omnibox.Focused)
		{
			omnibox.Select();
			try
			{
				SendKeys.Send(e.KeyChar.ToString());
			}
			catch
			{
				SendKeys.Send($"{{{e.KeyChar}}}");
			}
		}
	}

	private void Execute()
	{
		bool altMode = false; // if true, *do not* modify current entry, even if selected
		errorLabel.Text = string.Empty;

		if (string.IsNullOrWhiteSpace(omnibox.Text))
		{
			if (historyView.Items.Count <= 1) return;
			else
			{
				omnibox.Text = historyView.GetLastNonEmptyEntry();  // for duplicating last entry if omnibox empty
				if (string.IsNullOrWhiteSpace(omnibox.Text)) return;
			}
		}
		else if (ModifierKeys.HasFlag(Keys.Alt) && historyView.SelectedItem != null) altMode = true;

		string errorText;

		if (historyView.SelectedItem != null && !altMode)
		{
			if (historyView.EditSelected(omnibox.Text, out errorText))
			{
				RecalculateEntries(historyView.SelectedIndex + 1);
				OmniboxSuccess();
			}
			else
			{
				OmniboxError(errorText, altMode);
			}
		}
		else
		{
			if (historyView.AddEntry(omnibox.Text, out errorText))
			{
				historyView.TopIndex = historyView.Items.Count - (historyView.Items.Count > 1 ? 2 : 1);
				OmniboxSuccess();
			}
			else
			{
				OmniboxError(errorText, altMode);
			}
		}

		_textIndex = 0;
		ViewerForm.RefreshEntries(Solver);

		void OmniboxSuccess()
		{
			historyView.SelectedClear();
			SendCursorRight();
			omnibox.Clear();
		}

		void OmniboxError(string errorText, bool altMode)
		{
			errorLabel.Text = errorText;
			SendKeys.Send("{BACKSPACE}");
			omnibox.SelectionStart = omnibox.Text.Length;
			_textIndex = omnibox.SelectionStart;

			if (historyView.SelectedItem != null && !altMode)
			{
				historyView.Items[historyView.SelectedIndex] = omnibox.Text;
			}
		}
	}

	private void RecalculateEntries(int start)
	{
		for (int i = start; i < historyView.Items.Count; i++)
		{
			if (historyView.Get(i).Expression != string.Empty)
			{
				historyView.Recalculate(i);
			}
		}
	}

	private void SendCursorRight()
	{
		if (omnibox.Text.Length > 0)
		{
			omnibox.SelectionStart = omnibox.Text.Length;
			_textIndex = omnibox.SelectionStart;
		}
	}

	public string OmniText { get => omnibox.Text; set => omnibox.Text = value; }

	private void SendString(string s, bool paren = false)
	{
		omnibox.Select();
		omnibox.ResetSelection();
		SendKeys.Send(s);
		if (paren)
		{
			SendKeys.Send("{(}");
		}
	}

	private void ResetSolver()
	{
		Solver = new();

		Solver.WriteLine = WriteLine;
		Solver.Write = Write;
		Solver.WriteError = WriteError;
		Solver.ReadLine = ReadLine;
		Solver.Flush = Flush;
		Solver.Clear = Clear;

		ViewerForm.RefreshEntries(Solver);
	}

	#region IOHooks

	private void WriteError(string s) => errorLabel.Text = s;
	private void WriteLine(string s) => Stdout.AppendLine(s);
	private void Write(string s) => Stdout.Append(s);
	private void Flush() => Clear();
	private void Clear() => Stdout.Clear();
	private static string ReadLine() => throw new PlatformNotSupportedException("Prompting is not supported by WingCalculator");

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

	public void SelectOmnibox()
	{
		omnibox.Select();
		SendCursorRight();
	}

	internal WindowStyle CurrentStyle => _darkMode ? WindowStyle.DarkMode : WindowStyle.LightMode;

#if DEBUG
	public void Error(string s) => errorLabel.Text += s;
#endif

#pragma warning restore IDE1006 // $$$ Bad, move this
}
