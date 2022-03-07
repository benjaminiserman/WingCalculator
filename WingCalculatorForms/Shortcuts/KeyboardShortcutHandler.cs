namespace WingCalculatorForms.Shortcuts;

internal class KeyboardShortcutHandler
{
	private List<Shortcut> _shortcuts = new();

	public void AddShortcut(Keys keyCode, Keys modifiers, Action action) => _shortcuts.Add(new(keyCode, modifiers, action));

	public void DeleteShortcut(Action action) => _shortcuts = _shortcuts.Where(kvp => kvp.Action != action).ToList();

	public void ExecuteShortcuts(Keys keyCode, Keys modifiers)
	{
		foreach (var shortcut in _shortcuts)
		{
			if (shortcut.KeyCode == keyCode && shortcut.Modifiers == modifiers)
			{
				shortcut.Action.Invoke();
			}
		}
	}

	private record struct Shortcut(Keys KeyCode, Keys Modifiers, Action Action);
}