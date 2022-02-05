namespace WingCalculatorShared;
using System;
using System.Linq;

internal static class Operators
{
	private static readonly Dictionary<string, int> _precedenceTiers = new List<string>()
	{
		"prompt",
		"exponential",
		"multiplicative",
		"additive",
		"shift",
		"relational",
		"equality",
		"bitwise_and",
		"bitwise_xor",
		"bitwise_or",
		"conditional_and",
		"conditional_or",
		"elvis",
		"assignment",
		"print",
		"semicolon",
	}.Select((s, i) => (s, i)).ToDictionary(x => x.s, x => x.i);

	private static readonly Dictionary<string, Operator> _operators = new List<Operator>()
	{
		new("?", (a, b, solver) => new PromptNode((IAssignable)a, b, solver), _precedenceTiers["prompt"]),

		new("**", (a, b, solver) => new BinaryNode(a, b, (x, y) => Math.Pow(x, y)), _precedenceTiers["exponential"]),

		new("*", (a, b, solver) => new BinaryNode(a, b, (x, y) => x * y), _precedenceTiers["multiplicative"]),
		new("/", (a, b, solver) => new BinaryNode(a, b, (x, y) => x / y), _precedenceTiers["multiplicative"]),
		new("%", (a, b, solver) => new BinaryNode(a, b, (x, y) => x % y), _precedenceTiers["multiplicative"]),
		new("//", (a, b, solver) => new BinaryNode(a, b, (x, y) => Math.Floor(x / y)), _precedenceTiers["multiplicative"]),

		new("+", (a, b, solver) => new BinaryNode(a, b, (x, y) => x + y), _precedenceTiers["additive"]),
		new("-", (a, b, solver) => new BinaryNode(a, b, (x, y) => x - y), _precedenceTiers["additive"]),

		new("<<", (a, b, solver) => new BinaryNode(a, b, (x, y) => (int)x << (int)y), _precedenceTiers["shift"]),
		new(">>", (a, b, solver) => new BinaryNode(a, b, (x, y) => (int)x >> (int)y), _precedenceTiers["shift"]),

		new("<", (a, b, solver) => new BinaryNode(a, b, (x, y) => x < y ? 1 : 0), _precedenceTiers["relational"]),
		new(">", (a, b, solver) => new BinaryNode(a, b, (x, y) => x > y ? 1 : 0), _precedenceTiers["relational"]),
		new("<=", (a, b, solver) => new BinaryNode(a, b, (x, y) => x <= y ? 1 : 0), _precedenceTiers["relational"]),
		new(">=", (a, b, solver) => new BinaryNode(a, b, (x, y) => x >= y ? 1 : 0), _precedenceTiers["relational"]),

		new("<?", (a, b, solver) => new BinaryNode(a, b, (x, y) => x < y ? x : y), _precedenceTiers["relational"]),
		new(">?", (a, b, solver) => new BinaryNode(a, b, (x, y) => x > y ? x : y), _precedenceTiers["relational"]),
		new("<=?", (a, b, solver) => new BinaryNode(a, b, (x, y) => x <= y ? x : y), _precedenceTiers["relational"]),
		new(">=?", (a, b, solver) => new BinaryNode(a, b, (x, y) => x >= y ? x : y), _precedenceTiers["relational"]),

		new("==", (a, b, solver) => new BinaryNode(a, b, (x, y) => x == y ? 1 : 0), _precedenceTiers["equality"]),
		new("!=", (a, b, solver) => new BinaryNode(a, b, (x, y) => x != y ? 1 : 0), _precedenceTiers["equality"]),

		new("&", (a, b, solver) => new BinaryNode(a, b, (x, y) => (int)x & (int)y), _precedenceTiers["bitwise_and"]),

		new("^", (a, b, solver) => new BinaryNode(a, b, (x, y) => (int)x ^ (int)y), _precedenceTiers["bitwise_xor"]),

		new("|", (a, b, solver) => new BinaryNode(a, b, (x, y) => (int)x | (int)y), _precedenceTiers["bitwise_or"]),

		new("&&", (a, b, solver) => new AndNode(a, b), _precedenceTiers["conditional_and"]),

		new("||", (a, b, solver) => new OrNode(a, b), _precedenceTiers["conditional_or"]),

		new("?:", (a, b, solver) => new ElvisNode(a, b), _precedenceTiers["elvis"]),

		new("**=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("**"), b)), _precedenceTiers["assignment"]),
		new("*=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("*"), b)), _precedenceTiers["assignment"]),
		new("/=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("/"), b)), _precedenceTiers["assignment"]),
		new("%=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("%"), b)), _precedenceTiers["assignment"]),
		new("//=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("//"), b)), _precedenceTiers["assignment"]),
		new("+=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("+"), b)), _precedenceTiers["assignment"]),
		new("-=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("-"), b)), _precedenceTiers["assignment"]),
		new("<<=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("<<"), b)), _precedenceTiers["assignment"]),
		new(">>=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new(">>"), b)), _precedenceTiers["assignment"]),
		new("&=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("&"), b)), _precedenceTiers["assignment"]),
		new("^=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("^"), b)), _precedenceTiers["assignment"]),
		new("|=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("|"), b)), _precedenceTiers["assignment"]),

		new("?=", (a, b, solver) => new ElvisAssignmentNode((IAssignable)a, b), _precedenceTiers["assignment"]),
		new("=", (a, b, solver) => new AssignmentNode((IAssignable)a, b), _precedenceTiers["assignment"]),

		new(":", (a, b, solver) => new BinaryNode(a, b, (x, y) =>
		{
			solver.Write(GetPrint(x, y));

			return x;
		}), _precedenceTiers["print"]),

		new("::", (a, b, solver) => new BinaryNode(a, b, (x, y) =>
		{
			solver.WriteLine(GetPrint(x, y));

			return x;
		}), _precedenceTiers["print"]),

		new(";", (a, b, solver) => new BinaryNode(a, b, (x, y) => y), _precedenceTiers["semicolon"]),

	}.ToDictionary(x => x.Symbol, x => x);

	private static string GetPrint(double x, double y) => y switch
	{
		-1 => "\n",
		0 => x.ToString(),
		1 => ((char)(int)x).ToString(),
		2 => Convert.ToString((int)x, 2),
		8 => Convert.ToString((int)x, 8),
		16 => Convert.ToString((int)x, 16),

		_ => throw new NotImplementedException($"Format {y} is not implemented.")
	};

	public static INode CreateNode(INode a, PreOperatorNode op, INode b, Solver solver = null) => _operators[op.Text].Construct(a, b, solver);

	public static int GetPrecedence(string symbol) => _operators[symbol].Precedence;

	record struct Operator(string Symbol, Func<INode, INode, Solver, INode> Construct, int Precedence, Associativity Associativity = Associativity.Left);

	enum Associativity { Left, Right }
}
