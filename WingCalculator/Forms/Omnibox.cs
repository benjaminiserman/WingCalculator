namespace WingCalculator.Forms;

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
}
