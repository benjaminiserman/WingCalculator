namespace WingCalculatorForms;

using System;
using System.Text;
using System.Windows.Forms;
using WingCalculatorShared;

public partial class MainForm : Form
{
	private Solver _solver = new();

	private int _bufferOffset = 0;
	private int _nextOffset = 0;
	private bool _skipSelect = false;
	private readonly StringBuilder _stdout = new();

	public MainForm()
	{
		InitializeComponent();

		KeyPreview = true;
		KeyPress += new KeyPressEventHandler(HandleKeyPress);

		historyView.DrawMode = DrawMode.OwnerDrawVariable;
		historyView.MeasureItem += historyView_MeasureItem;
		historyView.DrawItem += historyView_DrawItem;
		historyView.Items.Add("\n\n");

		_solver.WriteLine = WriteLine;
		_solver.Write = Write;
		_solver.WriteError = WriteError;
		_solver.ReadLine = ReadLine;
	}

#pragma warning disable IDE1006 // Naming Styles
	#region Buttons
	private void bin_button_Click(object sender, EventArgs e) => SendKeys.Send("2:");

	private void pi_button_Click(object sender, EventArgs e) => SendKeys.Send("I$P");

	private void clr_button_Click(object sender, EventArgs e) => omnibox.Clear();

	private void exe_button_Click(object sender, EventArgs e) => Execute();

	private void hex_button_Click(object sender, EventArgs e) => SendKeys.Send("6:1");

	private void tau_button_Click(object sender, EventArgs e) => SendKeys.Send("U$TA");

	private void ans_button_Click(object sender, EventArgs e) => SendKeys.Send("S$AN");

	private void ac_button_Click(object sender, EventArgs e)
	{
		_solver = new();
		historyView.Items.Clear();
		historyView.Items.Add("\n\n");
		omnibox.Clear();
	}

	private void dec_button_Click(object sender, EventArgs e) => SendKeys.Send("0:");

	private void e_button_Click(object sender, EventArgs e) => SendKeys.Send("E$");

	private void sqrt_button_Click(object sender, EventArgs e) => SendKeys.Send("tsqr");

	private void cbrt_button_Click(object sender, EventArgs e) => SendKeys.Send("tcbr");

	private void txt_button_Click(object sender, EventArgs e) => SendKeys.Send("1:");

	private void arcsin_button_Click(object sender, EventArgs e) => SendKeys.Send("narcsi");

	private void arccos_button_Click(object sender, EventArgs e) => SendKeys.Send("sarcco");

	private void arctan_button_Click(object sender, EventArgs e) => SendKeys.Send("narcta");

	private void sin_button_Click(object sender, EventArgs e) => SendKeys.Send("nsi");

	private void cos_button_Click(object sender, EventArgs e) => SendKeys.Send("sco");

	private void tan_button_Click(object sender, EventArgs e) => SendKeys.Send("nta");

	private void pow_button_Click(object sender, EventArgs e) => SendKeys.Send("wpo");

	private void var_button_Click(object sender, EventArgs e) => SendKeys.Send("$");

	private void mac_button_Click(object sender, EventArgs e) => SendKeys.Send("@");

	private void ln_button_Click(object sender, EventArgs e) => SendKeys.Send("nl");

	private void log_button_Click(object sender, EventArgs e) => SendKeys.Send("glo");

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
		
		if ((string)historyView.Items[^1] != "\n\n") historyView.Items.Add("\n\n"); // add empty buffer entry

		for (int i = 0; i < historyView.Items.Count - 1; i++) // remove empty buffer entries that aren't at the end
		{
			if ((string)historyView.Items[i] == "\n\n")
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

	#region HistoryViewDrawing
	private void historyView_MeasureItem(object sender, MeasureItemEventArgs e) => e.ItemHeight = (int)e.Graphics.MeasureString(historyView.Items[e.Index].ToString(), historyView.Font, historyView.Width).Height;

	private void historyView_DrawItem(object sender, DrawItemEventArgs e)
	{
		try
		{
			e.DrawBackground();
			e.DrawFocusRectangle();
			e.Graphics.DrawString(historyView.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
		}
		catch { }
	}

	private void historyView_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (_skipSelect) _skipSelect = false;
		else if (historyView.SelectedItem is not null)
		{
			omnibox.Text = GetEntryText(historyView.SelectedItem);
		}
	}
	#endregion

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
			throw;
		}
	}

	#region IOHooks

	private void WriteError(string s) => errorLabel.Text = s;
	private void WriteLine(string s) => _stdout.AppendLine(s);
	private void Write(string s) => _stdout.Append(s);
	private static string ReadLine() => throw new PlatformNotSupportedException("Prompting is not available in the WinForms app.");

	#endregion
}
