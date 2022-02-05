namespace WingCalculatorForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class HistoryView : ListBox
{
	public HistoryView()
	{
		DrawMode = DrawMode.OwnerDrawVariable;
		MeasureItem += OnMeasureItem;
		DrawItem += OnDrawItem;
		Items.Add(MainForm.emptyEntry);
	}

	public void RefreshEntries() => RecreateHandle();

	#region HistoryViewDrawing
	private void OnMeasureItem(object sender, MeasureItemEventArgs e) => e.ItemHeight = (int)e.Graphics.MeasureString(Items[e.Index].ToString(), Font, Width).Height;

	private void OnDrawItem(object sender, DrawItemEventArgs e)
	{
		try
		{
			e.DrawBackground();
			e.DrawFocusRectangle();
			e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
		}
		catch { }
	}
	#endregion
}
