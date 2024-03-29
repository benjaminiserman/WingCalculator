﻿namespace WingCalculator.Forms.History;
using System;
using System.Windows.Forms;
using WingCalculator.Shortcuts;

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

		RegisterShortcuts();

		OnChange();
	}

	public void RefreshEntries() => RecreateHandle();

	public void RegisterShortcuts() => ShortcutActionRegistry.AddRange(new List<(string, Action)>()
	{
		("insert above", () => InsertAbove()),
		("insert below", () => InsertBelow()),
		("pop out", () => PopOut()),
		("pop out last", () => PopOutLast()),

		("copy expression", () => CopyExpression()),
		("copy output", () => CopyOutput()),
		("copy solution", () => CopySolution()),
		("copy error", () => CopyError()),
		("copy stack trace", () => CopyStackTrace()),
		("copy entire entry", () => CopyEntireEntry()),
		("execute all", () => Get(0).Solve(recalculate: true)),
	});

	public string SelectedUp(string omniText)
	{
		if (SelectedIndex == -1)
		{
			if (Items.Count == 1)
			{
				return SelectedChange(0, omniText);
			}

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
			OnChange();
		}

		SelectHandled = true;
		SelectedIndex = i;
		_trackedIndex = SelectedIndex;

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
		if (SelectHandled)
		{
			SelectHandled = false;
		}
		else if (SelectedIndex == -1)
		{
			return;
		}
		else // mouse click
		{
			if (_trackedIndex == -1)
			{
				_trackedIndex = Items.Count - 1;
			}

			if (_trackedIndex < Items.Count && Get(_trackedIndex).Expression != _mainForm.OmniText
				&& !string.IsNullOrWhiteSpace(_mainForm.OmniText))
			{
				Get(_trackedIndex).Expression = _mainForm.OmniText;
			}

			_trackedIndex = SelectedIndex;

			_mainForm.OmniText = Get(SelectedIndex).Expression;
			_mainForm.SelectOmnibox();

			//OnChange();
		}
	}

	public bool AddEntry(string s, out string error)
	{
		SelectedClear();
		HistoryEntry entry = new(_mainForm, this) { Expression = s };
		if (entry.Solve(true))
		{
			Items.Insert(Items.Count - 1, entry);
			//OnChange();
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
		//OnChange();
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
			//OnChange();
		}
	}

	public void Clear()
	{
		Items.Clear();
		OnChange();
	}

	public string DeleteSelected()
	{
		if (Items.Count > 1 && SelectedItem is not null && !string.IsNullOrWhiteSpace(GetSelected().Expression))
		{
			var index = SelectedIndex;
			GetSelected().Delete();
			Items.RemoveAt(index);

			return SelectedChange(index < Items.Count ? index : Items.Count - 1, null);
		}

		return string.Empty;
	}

	public string GetLastNonEmptyEntry() => Items.Count == 1 ? Get(^1).Expression : Get(^2).Expression;

	public void OnChange()
	{
		for (var i = 0; i < Items.Count - 1; i++) // remove empty buffer entries that aren't at the end
		{
			if (string.IsNullOrWhiteSpace(Get(i).Expression))
			{
				Items.RemoveAt(i);
				i--;
			}
		}

		if (Items.Count == 0 || !string.IsNullOrWhiteSpace(Get(^1).Expression))
		{
			Items.Add(new HistoryEntry(_mainForm, this) { Expression = string.Empty }); // add empty buffer entry
		}

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

	public double GetPreviousSolution(HistoryEntry entry)
	{
		var lastSolution = 0.0;
		for (var i = 0; i < Items.Count; i++)
		{
			if (Items[i] == entry)
			{
				return lastSolution;
			}
			else
			{
				lastSolution = ((HistoryEntry)Items[i]).Solution;
			}
		}

		if (Items.Count >= 2)
		{
			return ((HistoryEntry)Items[^2]).Solution;
		}
		else
		{
			return default;
		}
	}

	private void MenuStripItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		switch (e.ClickedItem.Text)
		{
			case "Insert Above":
			{
				InsertAbove();
				break;
			}
			case "Insert Below":
			{
				InsertBelow();
				break;
			}
			case "Pop Out":
			{
				PopOut();
				break;
			}
			case "Pop Out Last":
			{
				PopOutLast();
				break;
			}
			case "Copy...":
			{
				_copyStrip.Show(MousePosition);
				break;
			}
			case "Delete Entry":
			{
				DeleteEntry();
				break;
			}
		}
	}

	private void InsertAbove()
	{
		InsertEntry("$ANS", SelectedIndex);
		SelectedChange(SelectedIndex, null);
	}

	private void InsertBelow() => InsertEntry("$ANS", SelectedIndex + 1);

	private void PopOut()
	{
		PopoutEntry popout = new(GetSelected(), _mainForm.CurrentStyle)
		{
			TopMost = _mainForm.TopMost
		};
		popouts.Add(popout);
		popout.Show();
	}

	private void PopOutLast()
	{
		PopoutEntry popout = new(this, _mainForm.CurrentStyle)
		{
			TopMost = _mainForm.TopMost
		};
		popouts.Add(popout);
		popout.Show();
	}

	private void DeleteEntry() => _mainForm.OmniText = DeleteSelected();

	private void CopyExpression() => DoCopy("Copy Expression");
	private void CopyOutput() => DoCopy("Copy Output");
	private void CopySolution() => DoCopy("Copy Solution");
	private void CopyError() => DoCopy("Copy Error");
	private void CopyStackTrace() => DoCopy("Copy Stack Trace");
	private void CopyEntireEntry() => DoCopy("Copy Entire Entry");

	private void CopyStripItemClicked(object sender, ToolStripItemClickedEventArgs e) => DoCopy(e.ClickedItem.Text);

	private void DoCopy(string text)
	{
		var entry = GetSelected();

		var s = text switch
		{
			"Copy Expression" => entry.Expression,
			"Copy Output" => entry.Output,
			"Copy Solution" => entry.SolutionString,
			"Copy Error" => entry.Error,
			"Copy Stack Trace" => entry.StackTrace,
			"Copy Entire Entry" => entry.Entry,
			_ => throw new NotImplementedException()
		};

		if (string.IsNullOrEmpty(s))
		{
			s = " ";
		}

		Clipboard.SetText(s);
	}

	public void RecalculateAfter(HistoryEntry start)
	{
		var startIndex = Items.IndexOf(start) + 1;

		if (startIndex != 0)
		{
			for (var i = startIndex; i < Items.Count; i++)
			{
				Get(i).Solve(recalculate: false);
			}
		}
	}
}