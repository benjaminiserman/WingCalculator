namespace WingCalculator;

using System.Text.Json;
using WingCalculator.Forms;
using WingCalculator.Forms.History;
using WingCalculator.Shortcuts;

internal static class Program
{
	private static string ConfigPath { get; } = @"config.json";
	private static Config Config { get; set; }

	internal static KeyboardShortcutHandler KeyboardShortcutHandler { get; set; }
	internal static Omnibox LastFocusedTextBox { get; set; }
	private static MainForm _mainForm;

	/// <summary>
	///  The main entry point for the application.
	/// </summary>
	[STAThread]
	private static void Main()
	{
		// To customize application configuration such as set high DPI settings or default font,
		// see https://aka.ms/applicationconfiguration.
		ApplicationConfiguration.Initialize();

		bool showError = false;

		if (File.Exists(ConfigPath))
		{
			Config = JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigPath));
		}
		else
		{
			Config = new();
		}

		KeyboardShortcutHandler = Config.ShortcutHandler;

		Application.ApplicationExit += OnExit;

		_mainForm = new MainForm(Config);

#if DEBUG
		if (showError) _mainForm.Error("Previous state could not be loaded and was discarded.");
#endif

		Application.Run(_mainForm);
	}

	private static void OnExit(object sender, EventArgs e)
	{
		Config.ShortcutHandler.FillUnassigned();
		Config.Entries = Config.HistoryViewItems.Cast<HistoryEntry>().Select(x => x.Expression).ToList();
		File.WriteAllText(ConfigPath, JsonSerializer.Serialize(Config, new JsonSerializerOptions() { WriteIndented = true }));
	}

	public static WingCalc.Solver GetSolver() => _mainForm.Solver;
}