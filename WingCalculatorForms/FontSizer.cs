namespace WingCalculatorForms;
using System.Windows.Forms;

internal static class FontSizer
{
	public static void ApplySize(Control.ControlCollection collection, Form form, float size)
	{
		foreach (Control c in collection)
		{
			if (c is TextBox or Label or ListBox or CheckBox) c.Font = new(c.Font.FontFamily, size);
			if (c is HistoryView hv) hv.RefreshEntries();	
			if (c is Panel panel) ApplySize(panel.Controls, form, size);	
		}

		if (form is MainForm mf)
		{
			ApplySize(mf.ViewerForm.Controls, mf.ViewerForm, size);
		}
	}
}
