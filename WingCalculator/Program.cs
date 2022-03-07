namespace WingCalculator;
using WingCalculator.Forms;
using WingCalculator.Shortcuts;

internal static class Program
{
	/// <summary>
	///  The main entry point for the application.
	/// </summary>
	[STAThread]
	static void Main()
	{
		// To customize application configuration such as set high DPI settings or default font,
		// see https://aka.ms/applicationconfiguration.
		ApplicationConfiguration.Initialize();
		KeyboardShortcutHandler shortcutHandler = new();
		Application.Run(new MainForm(shortcutHandler));
	}
}