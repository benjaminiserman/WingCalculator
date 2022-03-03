namespace WingCalculatorForms.History;
using System.Windows.Forms;
using System.Text.RegularExpressions;

internal partial class PopoutEntry : Form
{
	private readonly HistoryEntry _entry;
	private readonly HistoryView _historyView;
	private bool _resized = false;

	private int _height, _width;

	private PopoutEntry(string s, WindowStyle style)
	{
		InitializeComponent();

		Update(s);
		style.Apply(Controls, this);
		ResizeEnd += (_, _) =>
		{
			if (Height != _height || Width != _width)
			{
				_resized = true;
			}
		};
	}

	public PopoutEntry(HistoryEntry entry, WindowStyle style) : this(entry.Entry, style)
	{
		_entry = entry;
		_entry.EntryChanged += () => Update(_entry.Entry);
		_entry.EntryDeleted += () => Close();
	}

	public PopoutEntry(HistoryView historyView, WindowStyle style) : this(historyView.GetLast().Entry, style)
	{
		_historyView = historyView;
		_historyView.UpdateLast += () => Update(_historyView.GetLast().Entry);
	}

	private void Update(string s)
	{
		textBox.Text = Regex.Replace(s, "(?<!\r)\n", "\r\n");
		DoResize(s);
	}

	public void DoResize(string s)
	{
		if (!_resized)
		{
			using Graphics g = CreateGraphics();
			SizeF size = g.MeasureString(s, Font);
			
			int guessWidth = (int)Math.Ceiling(size.Width);
			Width = guessWidth;

			if (Width < MinimumSize.Width) Width = MinimumSize.Width;
			if (Width > 1000) Width = 1000;

			size = g.MeasureString(s, Font, Width);
			int guessHeight = Font.Height * s.Count(c => c == '\n') + (int)Math.Ceiling(size.Height);
			Height = guessHeight + RectangleToScreen(ClientRectangle).Top - Top + 5; // add word wrap read, also min word wrap allowed

			if (Height < MinimumSize.Height) Height = MinimumSize.Height;
			if (Height > 800) Height = 800;


			_height = Height;
			_width = Width;
		}
	}
}
