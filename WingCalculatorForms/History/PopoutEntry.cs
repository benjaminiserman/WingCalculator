namespace WingCalculatorForms.History;
using System.Windows.Forms;
using System.Text.RegularExpressions;

internal partial class PopoutEntry : Form
{
	private readonly HistoryEntry _entry;
	private readonly HistoryView _historyView;

	private PopoutEntry(string s, WindowStyle style)
	{
		InitializeComponent();

		Update(s);
		style.Apply(Controls, this);


		int guessHeight = Font.Height * (s.Count(c => c == '\n') + 2);
		int guessWidth = (int)Math.Ceiling(CreateGraphics().MeasureString(s, Font).Width);

		Height = guessHeight + Top + 5;
		Width = guessWidth;

		if (Height < MinimumSize.Height) Height = MinimumSize.Height;
		if (Width < MinimumSize.Width) Width = MinimumSize.Width;

		if (Height > 800) Height = 800;
		if (Width > 500) Width = 1000;
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

	private void Update(string s) => textBox.Text = Regex.Replace(s, "(?<!\r)\n", "\r\n");
}
