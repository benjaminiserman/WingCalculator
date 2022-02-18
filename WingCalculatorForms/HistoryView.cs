namespace WingCalculatorForms;

using System;

internal class HistoryView : ListBox
{
	private static readonly string _emptyEntry = "\n\n";

	public bool SelectHandled { get; set; } = false;

	private int _trackedIndex = -1;
	private MainForm _mainForm;

	public HistoryView()
	{
		DrawMode = DrawMode.OwnerDrawVariable;
		MeasureItem += OnMeasureItem;
		DrawItem += OnDrawItem;
		Items.Add(_emptyEntry);
	}

	public void Connect(MainForm mainForm) => _mainForm = mainForm;

	public void RefreshEntries() => RecreateHandle();

	public string SelectedUp(string omniText)
	{
		if (SelectedIndex == -1)
		{
			if (Items.Count == 1) return SelectedChange(0, omniText);
			return SelectedChange(Items.Count - 2, omniText);
		}
		else if (SelectedIndex <= 0)
		{
			return SelectedChange(Items.Count - 1, omniText);
		}
		else
		{
			return SelectedChange(SelectedIndex - 1, omniText);
		}
	}

	public string SelectedDown(string omniText)
	{
		if (SelectedIndex == -1)
		{
			return SelectedChange(0, omniText);
		}
		else if (SelectedIndex >= Items.Count - 1)
		{
			return SelectedChange(0, omniText);
		}
		else
		{
			return SelectedChange(SelectedIndex + 1, omniText);
		}
	}

	public string SelectedChange(int i, string omniText)
	{
		if (SelectedIndex == -1)
		{
			SelectHandled = true;
			SelectedIndex = Items.Count - 1;
			_trackedIndex = SelectedIndex;
		}

		if (GetEntryText(Items[SelectedIndex]) != omniText && !string.IsNullOrWhiteSpace(omniText))
		{
			Items[SelectedIndex] = omniText;
		}

		SelectHandled = true;
		SelectedIndex = i;
		_trackedIndex = SelectedIndex;

		return GetEntryText(i);
	}

	public void SelectedClear()
	{
		if (SelectedIndex != -1)
		{
			SelectHandled = true;
			SelectedIndex = -1;
			_trackedIndex = SelectedIndex;
		}
	}

	protected override void OnSelectedIndexChanged(EventArgs e)
	{
		if (SelectHandled) SelectHandled = false;
		else if (SelectedIndex == -1) return;
		else
		{
			if (_trackedIndex == -1) _trackedIndex = Items.Count - 1;

			if (GetEntryText(Items[_trackedIndex]) != _mainForm.OmniText
				&& !string.IsNullOrWhiteSpace(_mainForm.OmniText))
			{
				SelectHandled = true;
				Items[SelectedIndex] = _mainForm.OmniText;
			}

			_trackedIndex = SelectedIndex;

			_mainForm.OmniText = GetEntryText(Items[SelectedIndex]);
			_mainForm.SelectOmnibox();
		}
	}

	public void AddEntry(string s)
	{
		SelectedClear();
		Items[^1] = s;
		RefillEntryBuffer();
	}

	public void EditSelected(string s) => Items[SelectedIndex] = s;

	public void EditAt(int i, string s) => Items[i] = s;

	public void Clear()
	{
		Items.Clear();
		RefillEntryBuffer();
	}

	public string DeleteSelected()
	{
		if (SelectedItem is not null && (string)SelectedItem != _emptyEntry)
		{
			int index = SelectedIndex;
			Items.Remove(SelectedItem);

			return SelectedChange(index < Items.Count ? index : Items.Count - 1, null);
		}

		return null;
	}

	public string GetEntryText(int i) => GetEntryText(Items[i]);

	private static string GetEntryText(object x) => ((string)x).Split('\n')[0];

	public string GetLastNonEmptyEntry() => GetEntryText(Items[^2]);

	private void RefillEntryBuffer()
	{
		for (int i = 0; i < Items.Count - 1; i++) // remove empty buffer entries that aren't at the end
		{
			if (string.IsNullOrWhiteSpace((string)Items[i]))
			{
				Items.RemoveAt(i);
				i--;
			}
		}

		if (Items.Count == 0 || (string)Items[^1] != _emptyEntry) Items.Add(_emptyEntry); // add empty buffer entry
	}

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
