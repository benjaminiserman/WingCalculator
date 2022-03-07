namespace WingCalculator.Shortcuts;

internal class KeyboardShortcutHandler
{
	private List<Shortcut> _shortcuts = new();

	public void AddShortcut(Keys keyCode, Keys modifiers, string action) => _shortcuts.Add(new(keyCode, modifiers, action));

	public void DeleteShortcut(string action) => _shortcuts = _shortcuts.Where(kvp => kvp.Action != action).ToList();

	public bool ExecuteShortcuts(Keys keyCode, Keys modifiers)
	{
		bool executed = false;

		foreach (var shortcut in _shortcuts)
		{
			if (shortcut.KeyCode == keyCode && shortcut.Modifiers == modifiers)
			{
				executed = true;
				ShortcutActionRegistry.Get(shortcut.Action).Invoke();
			}
		}

		return executed;
	}

	private record struct Shortcut(Keys KeyCode, Keys Modifiers, string Action);
}