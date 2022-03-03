namespace WingCalculatorForms.History;
using System.Text;
using WingCalculatorShared;
using WingCalculatorShared.Exceptions;

internal class HistoryEntry
{
	private string _expression;
	public string Expression 
	{ 
		get => _expression; 
		set
		{
			_expression = value.Trim();
			if (string.IsNullOrEmpty(_expression)) _expression = "\r\n\r\n";
			EntryChanged?.Invoke();
		}
	}

	public string Output { get; private set; }
	public string Solution { get; private set; }
	public string Error { get; private set; }
	public string StackTrace { get; private set; }

	public event Action EntryChanged;
	public event Action EntryDeleted;

	public bool Solve(Solver solver, StringBuilder stdout)
	{
		ClearAllOutput();
		bool impliedAns = false;
		

		if (string.IsNullOrWhiteSpace(Expression)) return true;

		try
		{
			double solve = solver.Solve(Expression, out impliedAns);
			Solution = solve.ToString();
		}
		catch (Exception ex)
		{
			Error = ex is WingCalcException or CustomException
				? ex.Message
				: $"{ex.GetType()}: {ex.Message}";

			if (ex is WingCalcException wx && wx.StackTrace != null) StackTrace = wx.StackTrace;

#if DEBUG
			StackTrace += $"\r\n{ex.StackTrace.Replace("&", "&&")}";
#endif
		}

		if (impliedAns)
		{
			for (int i = 0; i < Expression.Length; i++) // yes, this is just to match whether or not they put spaces on their operators.
			{
				if ("~!%^&*-+=|<>/;:?".Contains(Expression[i])) continue;
				else if (Expression[i] == ' ')
				{
					Expression = $"$ANS {Expression}";
				}
				else
				{
					Expression = $"$ANS{Expression}";
				}

				break;
			}
		}

		Output = stdout.ToString();
		stdout.Clear();

		return Error == string.Empty;
	}

	public void Delete() => EntryDeleted?.Invoke();

	public string Entry
	{
		get
		{
			if (string.IsNullOrWhiteSpace(Expression)) return "\r\n\r\n";

			StringBuilder sb = new();

			sb.AppendLine(Expression);

			if (Output != string.Empty) sb.AppendLine($"Output: {Output}");
			if (Solution != string.Empty) sb.AppendLine($"Solution: {Solution}");
			if (Error != string.Empty) sb.AppendLine($"Error: {Error}");
			if (StackTrace != string.Empty) sb.AppendLine($"StackTrace: {StackTrace}");

			sb = sb.Replace("\n", "\n> ");

			string s = sb.ToString();

			if (s.Length > 3 && s[^2..] == "> ") s = s[..^3];

			return s;
		}
	}

	public string FullError
	{
		get
		{
			if (string.IsNullOrWhiteSpace(Error)) return string.Empty;

			string error = $"Error: {Error}";
			if (StackTrace != string.Empty) error += $"\r\nStack Trace: {StackTrace}";

			return error;
		}
	}

	public void ClearAllOutput()
	{
		Output = string.Empty;
		Solution = string.Empty;
		Error = string.Empty;
		StackTrace = string.Empty;
	}
}
