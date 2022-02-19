namespace WingCalculatorShared;
using System;
using System.Linq;
using WingCalculatorShared.Exceptions;
using WingCalculatorShared.Nodes;

internal static class Operators
{
	private static readonly Dictionary<string, int> _precedenceTiers = new List<string>()
	{
		"prompt",
		"exponential",
		"coefficient",
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

		new("coeff", (a, b, solver) => new BinaryNode(a, b, (x, y) => x * y), _precedenceTiers["coefficient"]),

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

		new(":", (a, b, solver) => new PrintNode(a, b, solver, false), _precedenceTiers["print"]),

		new("::", (a, b, solver) => new PrintNode(a, b, solver, true), _precedenceTiers["print"]),

		new(";", (a, b, solver) => new BinaryNode(a, b, (x, y) => y), _precedenceTiers["semicolon"]),

	}.ToDictionary(x => x.Symbol, x => x);



	public static INode CreateNode(INode a, PreOperatorNode op, INode b, Solver solver = null)
	{
		try
		{
			return _operators[op.Text].Construct(a, b, solver);
		}
		catch (KeyNotFoundException)
		{
			throw new WingCalcException($"\"{op.Text}\" is not a valid binary operator.");
		}
		catch (InvalidCastException)
		{
			if (GetPrecedence(op.Text) == _precedenceTiers["assignment"])
			{
				throw new WingCalcException($"Operator {op.Text} was unable to cast some parameter(s). Make sure its left operand is something that can be assigned to.");
			}
			else
			{
				throw new WingCalcException($"Operator {op.Text} was unable to cast some parameter(s).");
			}
		}
	}

	public static int GetPrecedence(string symbol)
	{
		try
		{
			return _operators[symbol].Precedence;
		}
		catch (KeyNotFoundException)
		{
			throw new WingCalcException($"\"{symbol}\" is not a valid binary operator.");
		}
	}

	record struct Operator(string Symbol, Func<INode, INode, Solver, INode> Construct, int Precedence);

	public static Associativity GetTierAssociativity(int tier) => tier == _precedenceTiers["assignment"] || tier == _precedenceTiers["elvis"] || tier == _precedenceTiers["exponential"] ? Associativity.Right : Associativity.Left;

	public enum Associativity { Left, Right }
}
