namespace WingCalculator;
using WingCalculator.Shortcuts;
using System.Windows.Forms;
using System.Text.Json.Serialization;

internal class Config
{
	public KeyboardShortcutHandler ShortcutHandler { get; set; } = KeyboardShortcutHandler.Default;

	public bool IsDarkMode { get; set; } = false;

	public int FontSize { get; set; } = 9;

	[JsonIgnore]
	public ListBox.ObjectCollection HistoryViewItems { get; set; }

	public List<string> Entries { get; set; } = new();
}
