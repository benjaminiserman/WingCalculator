namespace WingCalculatorForms.History;
using System.Text.RegularExpressions;
using System.Windows.Forms;

internal partial class PopoutEntry : Form
{
	private HistoryEntry _entry;
	private bool _resized = false;
	private bool _canEdit = false;

	private readonly bool _isLockedToLast = false;
	private readonly HistoryView _lockedHistoryView = null;

	private int _height, _width;

	private readonly Func<string> _getText;

	private PopoutEntry(WindowStyle style)
	{
		InitializeComponent();

		style.Apply(Controls, this);
		ResizeEnd += DetectResize;

		_getText = () =>
		{
			string s = _canEdit
				? _entry.Expression
				: _entry.Entry.Trim();

			return Regex.Replace(s, "(?<!\r)\n", "\r\n");
		};

		editToggle.CheckedChanged += EditToggled;
		exeButton.Click += Execute;
		omniBox.TextChanged += OmniboxTextChanged;
	}

	public PopoutEntry(HistoryEntry entry, WindowStyle style) : this(style)
	{
		_entry = entry;
		_entry.EntryChanged += UpdateText;
		_entry.EntryDeleted += DoClose;

		UpdateText();
	}

	public PopoutEntry(HistoryView historyView, WindowStyle style) : this(style)
	{
		_isLockedToLast = true;
		_lockedHistoryView = historyView;
		_entry = _lockedHistoryView.GetLast();
		_lockedHistoryView.UpdateLast += UpdateLast;

		UpdateText();
	}

	private void UpdateText()
	{
		omniBox.Text = _getText();
		DoResize(omniBox.Text);
	}

	private void DoResize(string s)
	{
		if (!_resized)
		{
			try
			{
				using Graphics g = CreateGraphics();
				SizeF size = g.MeasureString(s, Font);

				int guessWidth = (int)Math.Ceiling(size.Width);
				Width = guessWidth;

				if (Width < MinimumSize.Width) Width = MinimumSize.Width;
				if (Width > 1000) Width = 1000;

				size = g.MeasureString(s, Font, Width);
				int guessHeight = Font.Height * s.Count(c => c == '\n') + (int)Math.Ceiling(size.Height);
				Height = guessHeight + RectangleToScreen(ClientRectangle).Top - Top + 45; // add word wrap read, also min word wrap allowed

				if (Height < MinimumSize.Height) Height = MinimumSize.Height;
				if (Height > 800) Height = 800;

				_height = Height;
				_width = Width;
			}
			catch // I do not know why this error is happening, so I'm just gonna ignore it
			{
				return;
			}
		}
	}

	private void EditToggled(object sender, EventArgs e)
	{
		_canEdit = !_canEdit;
		omniBox.ReadOnly = !_canEdit;

		if (!_canEdit)
		{
			_entry.Expression = omniBox.Text;
			_entry.RequestRefresh();
		}

		UpdateText();
	}

	private void Execute(object sender, EventArgs e)
	{
		if (_canEdit)
		{
			editToggle.Checked = false;
		}

		_entry.Solve(true);

		UpdateText();
		_entry.RequestRefresh();
	}

	private void DetectResize(object sender, EventArgs e)
	{
		if (Height != _height || Width != _width)
		{
			_resized = true;
		}
	}

	private void OmniboxTextChanged(object sender, EventArgs e)
	{
		if (_canEdit)
		{
			DoResize(omniBox.Text);
			_entry.SetOmniboxIfSelected(omniBox.Text);
		}
	}

	private void UpdateLast(HistoryView historyView)
	{
		_entry = historyView.GetLast();
		UpdateText();
	}

	private void DoClose()
	{
		if (_canEdit)
		{
			editToggle.Checked = false;
		}

		ResizeEnd -= DetectResize;
		editToggle.CheckedChanged -= EditToggled;
		exeButton.Click -= Execute;
		omniBox.TextChanged -= OmniboxTextChanged;

		if (_isLockedToLast)
		{
			_lockedHistoryView.UpdateLast -= UpdateLast;
		}
		else
		{
			_entry.EntryChanged -= UpdateText;
			_entry.EntryDeleted -= Close;
		}

		Close();
	}
}
