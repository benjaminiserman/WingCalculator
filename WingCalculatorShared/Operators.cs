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
		new("?", (a, b) => new PromptNode((IAssignable)a, b), _precedenceTiers["prompt"], PromptNode.Documentation),

		new("**", (a, b) => new BinaryNode(a, b, _operations["**"]), _precedenceTiers["exponential"], "Computes its left-hand operand to the power of its right-hand operand."),

		new("coeff", (a, b) => new BinaryNode(a, b, _operations["*"]), _precedenceTiers["coefficient"], "Computes the product of its operands. This operator is added by the interpreter where necessary to ensure expected behavior of coefficients. It cannot be manually added by the user."),

		new("*", (a, b) => new BinaryNode(a, b, _operations["*"]), _precedenceTiers["multiplicative"], "Computes the product of its operands."),
		new("/", (a, b) => new BinaryNode(a, b, _operations["/"]), _precedenceTiers["multiplicative"], "Computes the quotient of its operands."),
		new("%", (a, b) => new BinaryNode(a, b, _operations["%"]), _precedenceTiers["multiplicative"], "Computes the remainder of its left-hand operand divided by its right-hand operand."),
		new("//", (a, b) => new BinaryNode(a, b, _operations["//"]), _precedenceTiers["multiplicative"], "Computes and rounds down the quotient of its operands."),

		new("+", (a, b) => new BinaryNode(a, b, _operations["+"]), _precedenceTiers["additive"], "Computes the sum of its operands."),
		new("-", (a, b) => new BinaryNode(a, b, _operations["-"]), _precedenceTiers["additive"], "Computes its left-hand operand minus its right-hand operand."),

		new("<<", (a, b) => new BinaryNode(a, b, (x, y) => (int)x << (int)y), _precedenceTiers["shift"], "Converts its operands to integers and computes the left bit shift of its left-hand operator by its right-hand operator."),
		new(">>", (a, b) => new BinaryNode(a, b, (x, y) => (int)x >> (int)y), _precedenceTiers["shift"], "Converts its operands to integers and computes the right bit shift of its left-hand operator by its right-hand operator."),

		new("<", (a, b) => new BinaryNode(a, b, (x, y) => x < y ? 1 : 0), _precedenceTiers["relational"], "Returns 1 if its left-hand operand is less than its right-hand operand and otherwise returns 0."),
		new(">", (a, b) => new BinaryNode(a, b, (x, y) => x > y ? 1 : 0), _precedenceTiers["relational"], "Returns 1 if its left-hand operand is greater than its right-hand operand and otherwise returns 0."),
		new("<=", (a, b) => new BinaryNode(a, b, (x, y) => x <= y ? 1 : 0), _precedenceTiers["relational"], "Returns 1 if its left-hand operand is less than or equal to its right-hand operand and otherwise returns 0."),
		new(">=", (a, b) => new BinaryNode(a, b, (x, y) => x >= y ? 1 : 0), _precedenceTiers["relational"], "Returns 1 if its left-hand operand is greater than or equal to its right-hand operand and otherwise returns 0."),

		new("==", (a, b) => new BinaryNode(a, b, (x, y) => x == y ? 1 : 0), _precedenceTiers["equality"], "Returns 1 if its left-hand operand is equal to its right-hand operand and otherwise returns 0."),
		new("!=", (a, b) => new BinaryNode(a, b, (x, y) => x != y ? 1 : 0), _precedenceTiers["equality"], "Returns 1 if its left-hand operand is not equal to its right-hand operand, and otherwise returns 0."),

		new("&", (a, b) => new BinaryNode(a, b, _operations["&"]), _precedenceTiers["bitwise_and"], "Converts its operands to integers and computes their bitwise and."),

		new("^", (a, b) => new BinaryNode(a, b, _operations["^"]), _precedenceTiers["bitwise_xor"], "Converts its operands to integers and computes their bitwise xor."),

		new("|", (a, b) => new BinaryNode(a, b, _operations["|"]), _precedenceTiers["bitwise_or"], "Converts its operands to integers and computes their bitwise or."),

		new("&&", (a, b) => new AndNode(a, b), _precedenceTiers["conditional_and"], "Evaluates its left-hand operand. It then returns 0 if its left-hand operand equals 0. Otherwise, it evaluates its right-hand operand. If either operand evaluates to 0, it returns 0. Otherwise, it returns 1."),

		new("||", (a, b) => new OrNode(a, b), _precedenceTiers["conditional_or"], "Evaluates its left-hand operand. It then returns 1 if its left-hand operand equals 1. Otherwise, it evaluates its right-hand operand. If both operands evaluate to 0, it returns 0. Otherwise, it returns 1."),

		new("?:", (a, b) => new ElvisNode(a, b), _precedenceTiers["elvis"], "Evaluates its left-hand operand. It then returns its left-hand operand if it does not evaluate to 0. Otherwise, it evaluates and returns its right-hand operand."),

		new("**=", (a, b) => new CompoundAssignmentNode((IAssignable)a, "**", b), _precedenceTiers["assignment"], "Computes the ** of its operands, and assigns that value to its left-hand operand and returns it."),
		new("*=", (a, b) => new CompoundAssignmentNode((IAssignable)a, "*", b), _precedenceTiers["assignment"], "Computes the * of its operands, and assigns that value to its left-hand operand and returns it."),
		new("/=", (a, b) => new CompoundAssignmentNode((IAssignable)a, "/", b), _precedenceTiers["assignment"], "Computes the / of its operands, and assigns that value to its left-hand operand and returns it."),
		new("%=", (a, b) => new CompoundAssignmentNode((IAssignable)a, "%", b), _precedenceTiers["assignment"], "Computes the % of its operands, and assigns that value to its left-hand operand and returns it."),
		new("//=", (a, b) => new CompoundAssignmentNode((IAssignable)a, "//", b), _precedenceTiers["assignment"], "Computes the // of its operands, and assigns that value to its left-hand operand and returns it."),
		new("+=", (a, b) => new CompoundAssignmentNode((IAssignable)a, "+", b), _precedenceTiers["assignment"], "Computes the + of its operands, and assigns that value to its left-hand operand and returns it."),
		new("-=", (a, b) => new CompoundAssignmentNode((IAssignable)a, "-", b), _precedenceTiers["assignment"], "Computes the - of its operands, and assigns that value to its left-hand operand and returns it."),
		new("<<=", (a, b) => new CompoundAssignmentNode((IAssignable)a, "<<", b), _precedenceTiers["assignment"], "Computes the << of its operands, and assigns that value to its left-hand operand and returns it."),
		new(">>=", (a, b) => new CompoundAssignmentNode((IAssignable)a, ">>", b), _precedenceTiers["assignment"], "Computes the >> of its operands, and assigns that value to its left-hand operand and returns it."),
		new("&=", (a, b) => new CompoundAssignmentNode((IAssignable)a, "&", b), _precedenceTiers["assignment"], "Computes the & of its operands, and assigns that value to its left-hand operand and returns it."),
		new("^=", (a, b) => new CompoundAssignmentNode((IAssignable)a, "^", b), _precedenceTiers["assignment"], "Computes the ^ of its operands, and assigns that value to its left-hand operand and returns it."),
		new("|=", (a, b) => new CompoundAssignmentNode((IAssignable)a, "|", b), _precedenceTiers["assignment"], "Computes the | of its operands, and assigns that value to its left-hand operand and returns it."),

		new("?=", (a, b) => new ElvisAssignmentNode((IAssignable)a, b), _precedenceTiers["assignment"], "Computes the ?: of its operands, and assigns that value to its left-hand operand and returns it."),
		new("=", (a, b) => new AssignmentNode((IAssignable)a, b), _precedenceTiers["assignment"], "Assigns its right-hand operand to its left-hand operand."),

		new(":", (a, b) => new PrintNode(a, b, false), _precedenceTiers["print"], PrintNode.Documentation),

		new("::", (a, b) => new PrintNode(a, b, true), _precedenceTiers["print"], "Acts like the ':' operator, but also prints a newline at the end."),

		new(";", (a, b) => new BinaryNode(a, b, (x, y) => y), _precedenceTiers["semicolon"], "Evaluates both operands and returns its right-hand operand."),

	}.ToDictionary(x => x.Symbol, x => x);

	private static readonly Dictionary<string, Func<double, double, double>> _operations = new()
	{
		["**"] = (x, y) => Math.Pow(x, y),
		["*"] = (x, y) => x * y,
		["/"] = (x, y) => x / y,
		["%"] = (x, y) => x % y,
		["//"] = (x, y) => Math.Floor(x / y),
		["+"] = (x, y) => x + y,
		["-"] = (x, y) => x - y,
		["<<"] = (x, y) => (int)x << (int)y,
		[">>"] = (x, y) => (int)x >> (int)y,
		["&"] = (x, y) => (int)x & (int)y,
		["^"] = (x, y) => (int)x ^ (int)y,
		["|"] = (x, y) => (int)x | (int)y,
	};

	public static INode CreateNode(INode a, PreOperatorNode op, INode b = null)
	{
		try
		{
			return _operators[op.Text].Construct(a, b);
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

	public static Func<double, double, double> GetOperatorFunction(string symbol) => _operations[symbol];

	record struct Operator(string Symbol, Func<INode, INode, INode> Construct, int Precedence, string Documentation);

	public static Associativity GetTierAssociativity(int tier) => tier == _precedenceTiers["assignment"] || tier == _precedenceTiers["elvis"] || tier == _precedenceTiers["exponential"] ? Associativity.Right : Associativity.Left;

	public static bool GetDocumentation(string op, out string result)
	{
		if (_operators.ContainsKey(op))
		{
			result = _operators[op].Documentation;
			return true;
		}
		else
		{
			result = null;
			return false;
		}
	}

	public static string ListOperators() => string.Join(", ", _operators.Keys);

	public enum Associativity { Left, Right }
}
