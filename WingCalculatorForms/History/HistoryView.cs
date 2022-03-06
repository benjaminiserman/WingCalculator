namespace WingCalculatorForms.History;
using System;
using System.Windows.Forms;

internal class HistoryView : ListBox
{
	public bool SelectHandled { get; set; } = false;

	private int _trackedIndex = -1;
	private readonly MainForm _mainForm;
	private readonly ContextMenuStrip _menuStrip, _copyStrip;

	public readonly List<PopoutEntry> popouts = new();

	public event Action<HistoryView> UpdateLast;

	public HistoryView(MainForm mainForm)
	{
		_mainForm = mainForm;

		DrawMode = DrawMode.OwnerDrawVariable;
		MeasureItem += OnMeasureItem;
		DrawItem += OnDrawItem;
		HorizontalScrollbar = false;

		_copyStrip = new();
		_copyStrip.Items.Add("Copy Expression");
		_copyStrip.Items.Add("Copy Output");
		_copyStrip.Items.Add("Copy Solution");
		_copyStrip.Items.Add("Copy Error");
		_copyStrip.Items.Add("Copy Stack Trace");
		_copyStrip.Items.Add("Copy Entire Entry");
		_copyStrip.ItemClicked += CopyStripItemClicked;

		_menuStrip = new();
		_menuStrip.Items.Add("Insert Above");
		_menuStrip.Items.Add("Insert Below");
		_menuStrip.Items.Add("Pop Out");
		_menuStrip.Items.Add("Pop Out Last");
		_menuStrip.Items.Add("Copy...");
		_menuStrip.Items.Add("Delete Entry");
		_menuStrip.ItemClicked += MenuStripItemClicked;
		ContextMenuStrip = _menuStrip;
		MouseUp += OnClick;

		OnChange();
	}

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

		if (omniText is not null && Get(SelectedIndex).Expression != omniText && !string.IsNullOrWhiteSpace(omniText))
		{
			Get(SelectedIndex).Expression = omniText;
		}

		SelectHandled = true;
		SelectedIndex = i;
		_trackedIndex = SelectedIndex;
		OnChange();

		return Get(i).Expression;
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
		else // mouse click
		{
			if (_trackedIndex == -1) _trackedIndex = Items.Count - 1;

			if (_trackedIndex < Items.Count && Get(_trackedIndex).Expression != _mainForm.OmniText
				&& !string.IsNullOrWhiteSpace(_mainForm.OmniText))
			{
				Get(_trackedIndex).Expression = _mainForm.OmniText;
			}

			_trackedIndex = SelectedIndex;

			_mainForm.OmniText = Get(SelectedIndex).Expression;
			_mainForm.SelectOmnibox();

			OnChange();
		}
	}

	public bool AddEntry(string s, out string error)
	{
		SelectedClear();
		HistoryEntry entry = new(_mainForm, this) { Expression = s };
		if (entry.Solve(true))
		{
			Items.Insert(Items.Count - 1, entry);
			OnChange();
			error = string.Empty;
			return true;
		}
		else
		{
			error = entry.FullError;
			return false;
		}
	}

	public void InsertEntry(string s, int i)
	{
		HistoryEntry entry = new(_mainForm, this) { Expression = s };
		entry.Solve(true);
		Items.Insert(i, entry);
		OnChange();
	}

	public bool EditSelected(string s, out string error) => EditAt(SelectedIndex, s, out error);

	public bool EditAt(int i, string s, out string error)
	{
		try
		{
			Get(i).Expression = s;
			if (Get(i).Solve(true))
			{
				error = string.Empty;
				return true;
			}
			else
			{
				error = Get(i).FullError;
				return false;
			}
		}
		finally
		{
			OnChange();
		}
	}

	public void Clear()
	{
		Items.Clear();
		OnChange();
	}

	public string DeleteSelected()
	{
		if (SelectedItem is not null && !string.IsNullOrWhiteSpace(GetSelected().Expression))
		{
			int index = SelectedIndex;
			GetSelected().Delete();
			Items.RemoveAt(index);

			return SelectedChange(index < Items.Count ? index : Items.Count - 1, null);
		}

		return null;
	}

	public string GetLastNonEmptyEntry() => Items.Count == 1 ? Get(^1).Expression : Get(^2).Expression;

	private void OnChange()
	{
		for (int i = 0; i < Items.Count - 1; i++) // remove empty buffer entries that aren't at the end
		{
			if (string.IsNullOrWhiteSpace(Get(i).Expression))
			{
				Items.RemoveAt(i);
				i--;
			}
		}

		if (Items.Count == 0 || !string.IsNullOrWhiteSpace(Get(^1).Expression)) Items.Add(new HistoryEntry(_mainForm, this) { Expression = string.Empty }); // add empty buffer entry

		RefreshEntries();
		UpdateLast?.Invoke(this);
	}

	#region HistoryViewDrawing
	private void OnMeasureItem(object sender, MeasureItemEventArgs e) => e.ItemHeight = (int)e.Graphics.MeasureString(Get(e.Index).Entry, Font, Width).Height;

	private void OnDrawItem(object sender, DrawItemEventArgs e)
	{
		try
		{
			e.DrawBackground();
			e.DrawFocusRectangle();
			e.Graphics.DrawString(Get(e.Index).Entry, e.Font, new SolidBrush(e.ForeColor), e.Bounds);
		}
		catch { }
	}
	#endregion

	private void OnClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			SelectedIndex = IndexFromPoint(e.Location);
			if (SelectedIndex != -1)
			{
				_menuStrip.Show();
			}
		}
	}

	public HistoryEntry Get(Index i) => (HistoryEntry)Items[i];
	public HistoryEntry GetSelected() => Get(SelectedIndex);
	public HistoryEntry GetLast() => Items.Count > 1 ? Get(^2) : Get(^1);

	private void MenuStripItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		switch (e.ClickedItem.Text)
		{
			case "Insert Above":
			{
				InsertEntry("$ANS", SelectedIndex);
				SelectedChange(SelectedIndex, null);
				break;
			}
			case "Insert Below":
			{
				InsertEntry("$ANS", SelectedIndex + 1);
				break;
			}
			case "Pop Out":
			{
				PopoutEntry popout = new(GetSelected(), _mainForm.CurrentStyle);
				popouts.Add(popout);
				popout.Show();
				break;
			}
			case "Pop Out Last":
			{
				PopoutEntry popout = new(this, _mainForm.CurrentStyle);
				popouts.Add(popout);
				popout.Show();
				break;
			}
			case "Copy...":
			{
				_copyStrip.Show(MousePosition);
				break;
			}
			case "Delete Entry":
			{
				_mainForm.OmniText = DeleteSelected();
				break;
			}
		}
	}

	private void CopyStripItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		HistoryEntry entry = GetSelected();

		string s = e.ClickedItem.Text switch
		{
			"Copy Expression" => entry.Expression,
			"Copy Output" => entry.Output,
			"Copy Solution" => entry.Solution,
			"Copy Error" => entry.Error,
			"Copy Stack Trace" => entry.StackTrace,
			"Copy Entire Entry" => entry.Entry,
			_ => throw new NotImplementedException()
		};

		if (string.IsNullOrEmpty(s)) s = " ";

		Clipboard.SetText(s);
	}

	public void RecalculateAfter(HistoryEntry start)
	{
		int startIndex = Items.IndexOf(start) + 1;

		if (startIndex != 0)
		{
			for (int i = startIndex; i < Items.Count; i++)
			{
				Get(i).Solve(recalculate: false);
			}
		}
	}
}