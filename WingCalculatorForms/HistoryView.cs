namespace WingCalculatorForms;
using System;
using System.Windows.Forms;

internal class HistoryView : ListBox
{
	private static readonly string _emptyEntry = "\r\n\r\n";

	public bool SelectHandled { get; set; } = false;

	private int _trackedIndex = -1;
	private MainForm _mainForm;
	private readonly ContextMenuStrip _menuStrip;

	public readonly List<PopoutEntry> popouts = new();

	public HistoryView()
	{
		DrawMode = DrawMode.OwnerDrawVariable;
		MeasureItem += OnMeasureItem;
		DrawItem += OnDrawItem;
		HorizontalScrollbar = false;
		Items.Add(_emptyEntry);
		_menuStrip = new();
		_menuStrip.Items.Add("Insert Above");
		_menuStrip.Items.Add("Insert Below");
		_menuStrip.Items.Add("Pop Out");
		_menuStrip.Items.Add("Pop Out Last");
		_menuStrip.Items.Add("Copy Solution");
		_menuStrip.Items.Add("Copy Output");
		_menuStrip.Items.Add("Copy Entry");
		_menuStrip.Items.Add("Delete Entry");
		_menuStrip.ItemClicked += _menuStrip_ItemClicked;
		ContextMenuStrip = _menuStrip;
		MouseUp += OnClick;
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

		if (omniText is not null && GetEntryText(Items[SelectedIndex]) != omniText && !string.IsNullOrWhiteSpace(omniText))
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
		else // mouse click
		{
			if (_trackedIndex == -1) _trackedIndex = Items.Count - 1;

			if (_trackedIndex < Items.Count && GetEntryText(Items[_trackedIndex]) != _mainForm.OmniText
				&& !string.IsNullOrWhiteSpace(_mainForm.OmniText))
			{
				Items[_trackedIndex] = _mainForm.OmniText;
			}

			_trackedIndex = SelectedIndex;

			_mainForm.OmniText = GetEntryText(Items[SelectedIndex]);
			_mainForm.SelectOmnibox();

			RefillEntryBuffer();
		}
	}

	public void AddEntry(string s)
	{
		SelectedClear();
		Items[^1] = s;
		RefillEntryBuffer();
	}

	public void InsertEntry(string s, int i)
	{
		Items.Insert(i, s);
		RefillEntryBuffer();
	}

	public void EditSelected(string s)
	{
		int index = SelectedIndex;
		SelectHandled = true;
		Items.Insert(index, s);
		Items.RemoveAt(index + 1);
		SelectHandled = true;
		SelectedIndex = -1;
		SelectHandled = true;
		SelectedIndex = index; // this is not my fault, WinForms is extremely broke.
		RefillEntryBuffer();
	}

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
			Items.RemoveAt(index);

			return SelectedChange(index < Items.Count ? index : Items.Count - 1, null);
		}

		return null;
	}

	public string GetEntryText(int i) => GetEntryText(Items[i]);

	private static string GetEntryText(object x)
	{
		string s = x.ToString();

		int outputIndex = s.IndexOf("\r\n> Output:");
		int solveIndex = s.IndexOf("\r\n> Solution:");
		int errorIndex = s.IndexOf("\r\n> Error:");

		if (outputIndex != -1) return s[..outputIndex];
		if (solveIndex != -1) return s[..solveIndex];
		if (errorIndex != -1) return s[..errorIndex];

		return s.Trim();
	}

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

		for (int i = 0; i < Items.Count; i++)
		{
			foreach (var popout in popouts)
			{
				if (popout.Index == i)
				{
					popout.Update(Items[i].ToString());
				}
			}
		}

		foreach (var popout in popouts)
		{
			if (popout.Index == -1)
			{
				popout.Update(Items[Items.Count > 1 ? ^2 : ^1].ToString());
			}
		}
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

	private void _menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		string s = SelectedItem.ToString();

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
			case "Copy Solution":
			{
				int solutionIndex = s.IndexOf("\r\n> Solution: ");
				Clipboard.SetText(s[(solutionIndex + "\r\n> Solution: ".Length)..]);
				break;
			}
			case "Pop Out":
			{
				PopoutEntry popout = new(s, SelectedIndex, _mainForm.CurrentStyle);
				popouts.Add(popout);
				popout.Show();
				break;
			}
			case "Pop Out Last":
			{
				PopoutEntry popout = new(s, -1, _mainForm.CurrentStyle);
				popouts.Add(popout);
				popout.Show();
				break;
			}
			case "Copy Output":
			{
				int outputIndex = s.IndexOf("\r\n> Output: ");
				int solutionIndex = s.IndexOf("\r\n> Solution: ");

				if (outputIndex == -1) Clipboard.SetText("(no output)");
				else
				{
					Clipboard.SetText(s[(outputIndex + "\r\n> Output: ".Length)..solutionIndex].Replace("\r\n>", "\r\n"));
				}

				break;
			}
			case "Copy Entry":
			{
				Clipboard.SetText(s);
				break;
			}
			case "Delete Entry":
			{
				_mainForm.OmniText = DeleteSelected();
				break;
			}
		}
	}
}
