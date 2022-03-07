namespace WingCalculator;
using WingCalculator.Shortcuts;

internal class Config
{
	public KeyboardShortcutHandler ShortcutHandler { get; set; } = new();

	public bool IsDarkMode { get; set; } = false;

	public int FontSize { get; set; } = 9;
}
