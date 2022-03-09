namespace WingCalculator.Forms;
using System;

internal class Omnibox : TextBox
{
	int _selectionStart, _selectionLength;

	public Omnibox() : base()
	{
		UpdateSelection();

		KeyUp += (_, _) => UpdateSelection();
		MouseUp += (_, _) => UpdateSelection();
	}

	private void UpdateSelection()
	{
		_selectionStart = SelectionStart;
		_selectionLength = SelectionLength;
	}

	public void ResetSelection()
	{
		SelectionStart = _selectionStart;
		SelectionLength = _selectionLength;
	}

	public void SendString(string s)
	{
		int selectionStart = SelectionStart;

		if (SelectionLength > 0)
		{
			Text = Text.Remove(SelectionStart, SelectionLength);

			SelectionLength = 0;
		}

		Text = Text.Insert(SelectionStart, s);

		SelectionStart = selectionStart + s.Length;
	}

	protected override void OnGotFocus(EventArgs e)
	{
		if (!ReadOnly)
		{
			Program.LastFocusedTextBox = this;
		}

		base.OnGotFocus(e);
	}

	protected override void OnReadOnlyChanged(EventArgs e)
	{
		if (!ReadOnly)
		{
			Program.LastFocusedTextBox = this;
		}

		base.OnReadOnlyChanged(e);
	}
}
