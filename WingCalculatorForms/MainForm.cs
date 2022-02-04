namespace WingCalculatorForms;

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WingCalculatorShared;

public partial class MainForm : Form
{
	private Solver _solver = new();

	private int _bufferOffset = 0;
	private int _nextOffset = 0;
	private bool _skipSelect = false;

	public MainForm()
	{
		InitializeComponent();

		KeyPreview = true;
		KeyPress += new KeyPressEventHandler(HandleKeyPress);

		historyView.DrawMode = DrawMode.OwnerDrawVariable;
		historyView.MeasureItem += historyView_MeasureItem;
		historyView.DrawItem += historyView_DrawItem;
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
		if (string.IsNullOrWhiteSpace(omnibox.Text)) return;

		if (omnibox.Text[0..2] == "\r\n") omnibox.Text = omnibox.Text[2..];
		string solveString = GetSolve(omnibox.Text);

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

	private void RecalculateEntries(int start)
	{
		for (int i = start; i < historyView.Items.Count; i++)
		{
			Recalculate(i);
		}
	}

	private void Recalculate(int i)
	{
		if (!string.IsNullOrWhiteSpace((string)historyView.Items[i])) historyView.Items[i] = GetSolve(GetEntryText(historyView.Items[i]));
	}

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
#pragma warning restore IDE1006

	private static string GetEntryText(object x) => x.ToString().Split('\n')[0];
	private string GetSolve(string s) => $"{s}\n> Solution: {_solver.Solve(s)}";
}
