namespace WingCalculator.Forms.History;
using System.Text;
using WingCalc;
using WingCalc.Exceptions;

internal class HistoryEntry
{
	private string _expression;
	public string Expression
	{
		get => _expression;
		set
		{
			_expression = value.Trim();
			EntryChanged?.Invoke();

			Output = string.Empty;
			SolutionString = string.Empty;
			Error = string.Empty;
			StackTrace = string.Empty;
		}
	}

	public string Output { get; private set; } = string.Empty;
	public string SolutionString { get; private set; } = string.Empty;
	public double Solution { get; private set; }
	public string Error { get; private set; } = string.Empty;
	public string StackTrace { get; private set; } = string.Empty;

	public event Action EntryChanged;
	public event Action EntryDeleted;

	private readonly Solver _solver;
	private readonly HistoryView _historyView;
	private readonly StringBuilder _stdout;
	private readonly MainForm _mainForm;

	private HistoryEntry() => throw new NotImplementedException();

	public HistoryEntry(MainForm mainForm, HistoryView historyView)
	{
		_solver = mainForm.Solver;
		_stdout = mainForm.Stdout;
		_historyView = historyView;
		_mainForm = mainForm;

		EntryChanged += () =>
		{
			if (historyView.SelectedItem == this)
			{
				_mainForm.OmniText = Expression;
			}
		};
	}

	public bool Solve(bool recalculate = true)
	{
		ClearAllOutput();
		var impliedAns = false;

		if (string.IsNullOrWhiteSpace(Expression))
		{
			return true;
		}

		try
		{
			_solver.SetVariable("ANS", _mainForm.historyView.GetPreviousSolution(this));
			var solve = _solver.Solve(Expression, out impliedAns);
			Solution = solve;
			SolutionString = solve.ToString();
		}
		catch (Exception ex)
		{
			Error = ex is WingCalcException or CustomException
				? ex.Message
				: $"{ex.GetType()}: {ex.Message}";

			if (ex is WingCalcException wx && wx.StackTrace != null)
			{
				StackTrace = wx.StackTrace;
			}

#if DEBUG
			StackTrace += $"\r\n{ex.StackTrace.Replace("&", "&&")}";
#endif
		}

		if (impliedAns)
		{
			for (var i = 0; i < _expression.Length; i++) // yes, this is just to match whether or not they put spaces on their operators.
			{
				if ("~!%^&*-+=|<>/;:?".Contains(_expression[i]))
				{
					continue;
				}
				else if (_expression[i] == ' ')
				{
					_expression = $"$ANS {_expression}";
				}
				else
				{
					_expression = $"$ANS{_expression}";
				}

				break;
			}
		}

		Output = _stdout.ToString();
		_stdout.Clear();

		EntryChanged?.Invoke();

		if (recalculate)
		{
			_historyView.RecalculateAfter(this);
			_historyView.OnChange();
		}

		return Error == string.Empty;
	}

	public void Delete() => EntryDeleted?.Invoke();

	public string Entry
	{
		get
		{
			if (string.IsNullOrWhiteSpace(Expression))
			{
				return "\r\n\r\n";
			}

			StringBuilder sb = new();

			sb.AppendLine(Expression);

			if (Output != string.Empty)
			{
				if (Output.EndsWith("\r\n"))
				{
					sb.AppendLine($"> Output: {Output[..^2].Replace("\n", "\n> ")}");
				}
				else if (Output.EndsWith("\n"))
				{
					sb.AppendLine($"> Output: {Output[..^1].Replace("\n", "\n> ")}");
				}
				else
				{
					sb.AppendLine($"> Output: {Output.Replace("\n", "\n> ")}");
				}
			}

			if (SolutionString != string.Empty)
			{
				sb.AppendLine($"> Solution: {SolutionString}");
			}

			if (Error != string.Empty)
			{
				sb.AppendLine($"> Error: {Error.Replace("\n", "\n> ")}");
			}

			if (StackTrace != string.Empty)
			{
				sb.AppendLine($"> StackTrace: {StackTrace.Replace("\n", "\n> ")}");
			}

			var s = sb.ToString();

			if (s.Length >= 2 && s[^2..] == "> ")
			{
				s = s[..^3];
			}

			return s;
		}
	}

	public string FullError
	{
		get
		{
			if (string.IsNullOrWhiteSpace(Error))
			{
				return string.Empty;
			}

			var error = $"Error: {Error}";
			if (StackTrace != string.Empty)
			{
				error += $"\r\nStack Trace: {StackTrace}";
			}

			return error;
		}
	}

	public void ClearAllOutput()
	{
		Output = string.Empty;
		SolutionString = string.Empty;
		Error = string.Empty;
		StackTrace = string.Empty;
	}

	public void SetOmniboxIfSelected(string s)
	{
		if (_historyView.SelectedItem == this)
		{
			_mainForm.OmniText = s;
		}
	}

	public void RequestRefresh() => _historyView.RefreshEntries();
}
