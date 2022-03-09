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

	public void AddShortcut(Keys keyCode, Keys modifiers, string action) => AddShortcut(new(modifiers, keyCode, action));

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
				if (shortcut.Action.StartsWith("input("))
				{
					if (shortcut.Action[^1] != ')') throw new Exception("input call must end with a closing parenthesis!");
					ShortcutActionRegistry.Input.Invoke(shortcut.Action["input(".Length..^1]);
				}
				else if (shortcut.Action.StartsWith("eval("))
				{
					if (shortcut.Action[^1] != ')') throw new Exception("eval call must end with a closing parenthesis!");

					Program.GetSolver().Solve(shortcut.Action["eval(".Length..^1], false);
				}
				else if (shortcut.Action.StartsWith("solve("))
				{
					if (shortcut.Action[^1] != ')') throw new Exception("solve call must end with a closing parenthesis!");
					ShortcutActionRegistry.Input.Invoke(Program.GetSolver().Solve(shortcut.Action["solve(".Length..^1], false).ToString());
				}
				else if (shortcut.Action.StartsWith("solvestring("))
				{
					if (shortcut.Action[^1] != ')') throw new Exception("solvestring call must end with a closing parenthesis!");

					int commaIndex = -1;
					int open = 0;
					for (int i = 0; i < shortcut.Action.Length; i++)
					{
						if (shortcut.Action[i] == '(') open++;
						else if (shortcut.Action[i] == ')') open--;
						else if (shortcut.Action[i] == ',' && open == 1)
						{
							commaIndex = i;
							break;
						}
					}

					if (commaIndex == -1) throw new Exception("solvestring call must have two arguments!");

					double pointer = Program.GetSolver().Solve(shortcut.Action["solvestring(".Length..commaIndex], false);
					Program.GetSolver().Solve(shortcut.Action[(commaIndex + 1)..^1], false);

					ShortcutActionRegistry.Input.Invoke(Program.GetSolver().GetString(pointer));
				}
				else if (shortcut.Action.StartsWith("copysolve("))
				{
					if (shortcut.Action[^1] != ')') throw new Exception("copysolve call must end with a closing parenthesis!");
					Clipboard.SetText(Program.GetSolver().Solve(shortcut.Action["copysolve(".Length..^1], false).ToString());
				}
				else if (shortcut.Action.StartsWith("copysolvestring("))
				{
					if (shortcut.Action[^1] != ')') throw new Exception("copysolvestring call must end with a closing parenthesis!");

					int commaIndex = -1;
					int open = 0;
					for (int i = 0; i < shortcut.Action.Length; i++)
					{
						if (shortcut.Action[i] == '(') open++;
						else if (shortcut.Action[i] == ')') open--;
						else if (shortcut.Action[i] == ',' && open == 1)
						{
							commaIndex = i;
							break;
						}
					}

					if (commaIndex == -1) throw new Exception("copysolvestring call must have two arguments!");

					double pointer = Program.GetSolver().Solve(shortcut.Action["copysolvestring(".Length..commaIndex], false);
					Program.GetSolver().Solve(shortcut.Action[(commaIndex + 1)..^1], false);

					Clipboard.SetText(Program.GetSolver().GetString(pointer));
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

	public static void ExecuteName(string name) => ShortcutActionRegistry.Get(name).Invoke();

	internal record struct Shortcut(
	[property: JsonConverter(typeof(JsonStringEnumConverter))] Keys Modifiers, [property: JsonConverter(typeof(JsonStringEnumConverter))] Keys KeyCode, string Action);

	public static KeyboardShortcutHandler Default => new(new List<Shortcut>()
	{
		new(Keys.Control, Keys.Oemplus, "increase font size"),
		new(Keys.Control, Keys.OemMinus, "decrease font size"),

		new(Keys.None, Keys.PageUp, "page up"),
		new(Keys.None, Keys.PageDown, "page down"),

		new(Keys.Alt, Keys.Back, "delete entry"),
		new(Keys.Alt, Keys.Delete, "delete all"),
		new(Keys.Shift, Keys.Back, "clear"),

		new(Keys.None, Keys.Enter, "execute"),
		new(Keys.Alt, Keys.Enter, "execute at end"),
		new(Keys.Alt | Keys.Shift, Keys.Enter, "execute all"),

		new(Keys.Alt, Keys.I, "input(ඞ)"),
		new(Keys.Alt, Keys.A, "input($ANS)"),
		new(Keys.Alt, Keys.P, "input($PI)"),
		new(Keys.Alt | Keys.Shift, Keys.A, "solve($ANS)"),

		new(Keys.Alt, Keys.X, "copy expression"),
		new(Keys.Alt, Keys.C, "copy solution"),
		new(Keys.Alt, Keys.O, "copy output"),
		new(Keys.Alt, Keys.N, "copy entire entry"),

		new(Keys.Alt, Keys.T, "anchor to top"),
	});
}