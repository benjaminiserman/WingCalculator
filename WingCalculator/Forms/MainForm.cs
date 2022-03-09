namespace WingCalculator.Forms;
using System;
using System.Text;
using System.Windows.Forms;
using WingCalc;
using WingCalculator.Properties;
using WingCalculator.Shortcuts;

public partial class MainForm : Form
{
	public Solver Solver { get; private set; }
	public ViewerForm ViewerForm { get; } = new();

	public StringBuilder Stdout { get; private set; } = new();
	private int _textIndex;
	private readonly Config _config;

	internal MainForm(Config config)
	{
		_config = config;

		ResetSolver();

		InitializeComponent();

		historyView.Clear();
		try
		{
			if (_config.Entries is not null)
			{
				foreach (var expression in _config.Entries)
				{
					historyView.AddEntry(expression, out _);
				}
			}
		}
		catch
		{
			errorLabel.Text = "Last state could not be loaded, and was discarded.";
			historyView.Clear();
		}

		foreach (var shortcut in KeyboardShortcutHandler.Default.GetBoundNames())
		{
			if (!_config.ShortcutHandler.ContainsBoundName(shortcut))
			{
				_config.ShortcutHandler.AddShortcut(KeyboardShortcutHandler.Default, shortcut);
			}
		}

		_config.HistoryViewItems = historyView.Items;

		FontSizer.ApplySize(Controls, this, _config.FontSize);

		if (config.IsDarkMode)
		{
			WindowStyle.DarkMode.Apply(Controls, this);
			darkModeButton.BackgroundImage = Resources.light_bulb;
		}

		RegisterShortcuts();
		ShortcutActionRegistry.Input = SendString;

		KeyPreview = true;
		KeyPress += new KeyPressEventHandler(FormControlKeys);

		ResizeEnd += (_, _) => historyView.RefreshEntries();
	}

#pragma warning disable IDE0053
	public void RegisterShortcuts() => ShortcutActionRegistry.AddRange(new List<(string, Action)>()
	{
		("increase font size", (Action)(() =>
		{
			_config.FontSize++;
			FontSizer.ApplySize(Controls, this, _config.FontSize);
		})),
		("decrease font size", (Action)(() =>
		{
			_config.FontSize--;
			FontSizer.ApplySize(Controls, this, _config.FontSize);
		})),
		("page up", (Action)(() =>
		{
			OmniText = historyView.SelectedChange(0, OmniText);
		})),
		("page down", (Action)(() =>
		{
			OmniText = historyView.SelectedChange(historyView.Items.Count - 1, OmniText);
		})),
		("entry up", (Action)(() =>
		{
			if (omnibox.SelectionStart != _textIndex)
			{
				return;
			}
			else
			{
				OmniText = historyView.SelectedUp(OmniText);
				SendCursorRight();
			}
		})),
		("entry down", (Action)(() =>
		{
			if (omnibox.SelectionStart != _textIndex)
			{
				return;
			}
			else
			{
				OmniText = historyView.SelectedDown(OmniText);
				SendCursorRight();
			}
		})),
		("delete entry", (Action)(() =>
		{
			OmniText = historyView.DeleteSelected();
		})),
		("delete all", (Action)(() =>
		{
			ac_button_Click(this, null);
		})),
		("execute", (Action)(() =>
		{
			Execute(false);
		})),
		("execute at end", (Action)(() =>
		{
			Execute(true);
		})),
		("clear", (Action)(() =>
		{
			clr_button_Click(this, null);
		})),
		("anchor to top", (Action)(() =>
		{
			ToggleAnchor();
		}))
	});
#pragma warning restore IDE0053

#pragma warning disable IDE1006 // Naming Styles
	#region CalculatorButtons
	private void bin_button_Click(object sender, EventArgs e) => SendString("::$BIN");

	private void pi_button_Click(object sender, EventArgs e) => SendString("$PI");

	private void clr_button_Click(object sender, EventArgs e)
	{
		omnibox.Clear();
		errorLabel.Text = string.Empty;
	}

	private void exe_button_Click(object sender, EventArgs e) => Execute(false);

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

	private void sqrt_button_Click(object sender, EventArgs e) => SendString("sqrt(");

	private void cbrt_button_Click(object sender, EventArgs e) => SendString("cbrt(");

	private void txt_button_Click(object sender, EventArgs e) => SendString("::$TXT");

	private void arcsin_button_Click(object sender, EventArgs e) => SendString("asin(");

	private void arccos_button_Click(object sender, EventArgs e) => SendString("acos(");

	private void arctan_button_Click(object sender, EventArgs e) => SendString("atan(");

	private void sin_button_Click(object sender, EventArgs e) => SendString("sin(");

	private void cos_button_Click(object sender, EventArgs e) => SendString("cos(");

	private void tan_button_Click(object sender, EventArgs e) => SendString("tan(");

	private void pow_button_Click(object sender, EventArgs e) => SendString("**");

	private void var_button_Click(object sender, EventArgs e) => SendString("$");

	private void mac_button_Click(object sender, EventArgs e) => SendString("@");

	private void ln_button_Click(object sender, EventArgs e) => SendString("ln(");

	private void log_button_Click(object sender, EventArgs e) => SendString("log(");

	#endregion

	#region MenuButtons

	private void darkModeButton_Click(object sender, EventArgs e)
	{
		_config.IsDarkMode = !_config.IsDarkMode;

		if (_config.IsDarkMode)
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

	private void lockButton_Click(object sender, EventArgs e) => ToggleAnchor();

	#endregion

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (Program.KeyboardShortcutHandler.ExecuteShortcuts(e.KeyCode, e.Modifiers))
		{
			e.SuppressKeyPress = true;
			e.Handled = true;
		}
		else
		{
			switch (e.KeyCode)
			{
				case Keys.Up:
				{
					KeyboardShortcutHandler.ExecuteName("entry up");
					break;
				}
				case Keys.Down:
				{
					KeyboardShortcutHandler.ExecuteName("entry down");
					break;
				}
			}
		}

		_textIndex = omnibox.SelectionStart;
	}

	private void FormControlKeys(object sender, KeyPressEventArgs e) // focus keys to omnibox, capture return
	{
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

	private void Execute(bool alt)
	{
		if (ModifierKeys.HasFlag(Keys.Shift)) return;

		bool altMode = false; // if true, *do not* modify current entry, even if selected
		errorLabel.Text = string.Empty;

		if (string.IsNullOrWhiteSpace(OmniText))
		{
			if (historyView.Items.Count <= 1) return;
			else
			{
				OmniText = historyView.GetLastNonEmptyEntry();  // for duplicating last entry if omnibox empty
				if (string.IsNullOrWhiteSpace(OmniText)) return;
			}
		}
		else if (alt && historyView.SelectedItem != null) altMode = true;

		string errorText;

		if (historyView.SelectedItem != null && !altMode)
		{
			if (historyView.EditSelected(OmniText, out errorText))
			{
				OmniboxSuccess();
			}
			else
			{
				OmniboxError(errorText);
			}
		}
		else
		{
			if (historyView.AddEntry(OmniText, out errorText))
			{
				historyView.TopIndex = historyView.Items.Count - (historyView.Items.Count > 1 ? 2 : 1);
				OmniboxSuccess();
			}
			else
			{
				OmniboxError(errorText);
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

		void OmniboxError(string errorText)
		{
			errorLabel.Text = errorText;
			omnibox.SelectionStart = OmniText.Length;
			_textIndex = omnibox.SelectionStart;
		}
	}

	private void SendCursorRight()
	{
		if (OmniText.Length > 0)
		{
			omnibox.SelectionStart = OmniText.Length;
			_textIndex = omnibox.SelectionStart;
		}
	}

	public string OmniText { get => omnibox.Text; set => omnibox.Text = value.Trim(); }

	private void SendString(string s)
	{
		if (Program.LastFocusedTextBox is null)
		{
			Program.LastFocusedTextBox = omnibox;
		}

		Program.LastFocusedTextBox.SendString(s);
		Program.LastFocusedTextBox.Focus();
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

	private void ToggleAnchor()
	{
		Hide();
		TopMost = !TopMost;

		ViewerForm.TopMost = TopMost;
		foreach (var popout in historyView.popouts)
		{
			popout.TopMost = TopMost;
		}

		Show();
	}

	internal WindowStyle CurrentStyle => _config.IsDarkMode ? WindowStyle.DarkMode : WindowStyle.LightMode;

#if DEBUG
	public void Error(string s) => errorLabel.Text += s;
#endif

#pragma warning restore IDE1006 // $$$ Bad, move this
}
