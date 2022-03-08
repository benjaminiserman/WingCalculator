namespace WingCalculator.Shortcuts;

internal static class ShortcutActionRegistry
{
	private static readonly Dictionary<string, Action> _actions = new();

	public static Action<string> Input { get; set; }

	public static void Add(string name, Action action) => _actions.Add(name, action);
	public static void AddRange(IEnumerable<(string name, Action action)> range)
	{
		foreach (var (name, action) in range)
		{
			Add(name, action);
		}
	}

	public static Action Get(string name) => _actions[name];
}
