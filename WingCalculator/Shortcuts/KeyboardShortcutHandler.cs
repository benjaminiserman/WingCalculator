namespace WingCalculator.Shortcuts;
using System.Linq;

internal class KeyboardShortcutHandler
{
	private List<Shortcut> _shortcuts = new();

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

	public void ExecuteName(string name) => ShortcutActionRegistry.Get(_shortcuts.First(x => x.Action == name).Action).Invoke();

	private record struct Shortcut(Keys KeyCode, Keys Modifiers, string Action);

	public static KeyboardShortcutHandler Default => new(new()
	{
		(Keys.Oemplus, Keys.Control, "increase font size"),
		(Keys.OemMinus, Keys.Control, "decrease font size"),
		(Keys.PageUp, Keys.None, "page up"),
		(Keys.PageDown, Keys.None, "page down"),
		(Keys.None, Keys.None, "entry up"),
		(Keys.None, Keys.None, "entry down"),
		(Keys.Back, Keys.Alt, "delete entry"),
		(Keys.Delete, Keys.Alt, "delete all"),
		(Keys.Enter, Keys.None, "execute"),
		(Keys.Enter, Keys.Alt, "execute at end"),
		(Keys.A, Keys.Alt, "input(ඞ)"),
	});
}