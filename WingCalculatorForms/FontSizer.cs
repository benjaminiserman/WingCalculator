namespace WingCalculatorForms;

internal static class FontSizer
{
	public static void ApplySize(Control.ControlCollection collection, Form form, float size)
	{
		foreach (Control control in collection)
		{
			if (control is TextBox or HistoryView or Label)
			{
				control.Font = new(control.Font.FontFamily, size);
			}

			if (control is Form f) ApplySize(collection, f, size);
		}

		if (form is MainForm mf)
		{
			ApplySize(mf.ViewerForm.Controls, mf.ViewerForm, size);
		}
	}
}
