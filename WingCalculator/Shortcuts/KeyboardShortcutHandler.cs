namespace WingCalculator.Shortcuts;
using System.Linq;
using System.Text.Json.Serialization;

internal class KeyboardShortcutHandler
{
	private List<Shortcut> _shortcuts = new();
	public List<Shortcut> Shortcuts => _shortcuts.Select(x => x).ToList();

	[JsonConstructor]
	public KeyboardShortcutHandler(List<Shortcut> shortcuts)
	{
		foreach (var x in shortcuts)
		{
			AddShortcut(x);
		}
	}

	public KeyboardShortcutHandler(List<(Keys, Keys, string)> shortcuts)
	{
		foreach (var (k, m, a) in shortcuts)
		{
			AddShortcut(k, m, a);
		}
	}

	public IEnumerable<string> GetBoundNames()
	{
		foreach (var x in _shortcuts)
		{
			yield return x.Action;
		}
	}

	public bool ContainsBoundName(string s) => _shortcuts.Any(x => x.Action == s);

	private void AddShortcut(Shortcut shortcut) => _shortcuts.Add(shortcut);

	public void AddShortcut(Keys keyCode, Keys modifiers, string action) => AddShortcut(new(keyCode, modifiers, action));

	public void AddShortcut(KeyboardShortcutHandler from, string name) => AddShortcut(from._shortcuts.First(x => x.Action == name));

	public void DeleteShortcut(string action) => _shortcuts = _shortcuts.Where(kvp => kvp.Action != action).ToList();

	public bool ExecuteShortcuts(Keys keyCode, Keys modifiers)
	{
		bool executed = false;

		foreach (var shortcut in _shortcuts)
		{
			if (shortcut.KeyCode == keyCode && shortcut.Modifiers == modifiers)
			{
				executed = true;
				if (shortcut.Action.StartsWith("input"))
				{
					ShortcutActionRegistry.Input.Invoke(shortcut.Action["input(".Length..^1]);
				}
				else
				{
					ShortcutActionRegistry.Get(shortcut.Action).Invoke();
				}
			}
		}

		return executed;
	}

	public KeyboardShortcutHandler FillUnassigned()
	{
		foreach (string name in ShortcutActionRegistry.GetNames())
		{
			if (!_shortcuts.Any(x => x.Action == name))
			{
				AddShortcut(Keys.None, Keys.None, name);
			}
		}

		return this;
	}

	public void ExecuteName(string name) => ShortcutActionRegistry.Get(_shortcuts.First(x => x.Action == name).Action).Invoke();

	internal record struct Shortcut(
	[property: JsonConverter(typeof(JsonStringEnumConverter))] Keys KeyCode, [property: JsonConverter(typeof(JsonStringEnumConverter))] Keys Modifiers, string Action);

	public static KeyboardShortcutHandler Default => new(new List<Shortcut>()
	{
		new(Keys.Oemplus, Keys.Control, "increase font size"),
		new(Keys.OemMinus, Keys.Control, "decrease font size"),
		new(Keys.PageUp, Keys.None, "page up"),
		new(Keys.PageDown, Keys.None, "page down"),
		new(Keys.None, Keys.None, "entry up"),
		new(Keys.None, Keys.None, "entry down"),
		new(Keys.Back, Keys.Alt, "delete entry"),
		new(Keys.Delete, Keys.Alt, "delete all"),
		new(Keys.Enter, Keys.None, "execute"),
		new(Keys.Enter, Keys.Alt, "execute at end"),

		new(Keys.A, Keys.Alt, "input(ඞ)"),
	});
}