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
		new("?", (a, b, solver) => new PromptNode((IAssignable)a, b, solver), _precedenceTiers["prompt"], PromptNode.Documentation),

		new("**", (a, b, solver) => new BinaryNode(a, b, (x, y) => Math.Pow(x, y)), _precedenceTiers["exponential"], "Computes its left-hand operand to the power of its right-hand operand."),

		new("coeff", (a, b, solver) => new BinaryNode(a, b, (x, y) => x * y), _precedenceTiers["coefficient"], "Computes the product of its operands. This operator is added by the interpreter where necessary to ensure expected behavior of coefficients. It cannot be manually added by the user."),

		new("*", (a, b, solver) => new BinaryNode(a, b, (x, y) => x * y), _precedenceTiers["multiplicative"], "Computes the product of its operands."),
		new("/", (a, b, solver) => new BinaryNode(a, b, (x, y) => x / y), _precedenceTiers["multiplicative"], "Computes the quotient of its operands."),
		new("%", (a, b, solver) => new BinaryNode(a, b, (x, y) => x % y), _precedenceTiers["multiplicative"], "Computes the remainder of its left-hand operand divided by its right-hand operand."),
		new("//", (a, b, solver) => new BinaryNode(a, b, (x, y) => Math.Floor(x / y)), _precedenceTiers["multiplicative"], "Computes and rounds down the quotient of its operands."),

		new("+", (a, b, solver) => new BinaryNode(a, b, (x, y) => x + y), _precedenceTiers["additive"], "Computes the sum of its operands."),
		new("-", (a, b, solver) => new BinaryNode(a, b, (x, y) => x - y), _precedenceTiers["additive"], "Computes its left-hand operand minus its right-hand operand."),

		new("<<", (a, b, solver) => new BinaryNode(a, b, (x, y) => (int)x << (int)y), _precedenceTiers["shift"], "Converts its operands to integers and computes the left bit shift of its left-hand operator by its right-hand operator."),
		new(">>", (a, b, solver) => new BinaryNode(a, b, (x, y) => (int)x >> (int)y), _precedenceTiers["shift"], "Converts its operands to integers and computes the right bit shift of its left-hand operator by its right-hand operator."),

		new("<", (a, b, solver) => new BinaryNode(a, b, (x, y) => x < y ? 1 : 0), _precedenceTiers["relational"], "Returns 1 if its left-hand operand is less than its right-hand operand and otherwise returns 0."),
		new(">", (a, b, solver) => new BinaryNode(a, b, (x, y) => x > y ? 1 : 0), _precedenceTiers["relational"], "Returns 1 if its left-hand operand is greater than its right-hand operand and otherwise returns 0."),
		new("<=", (a, b, solver) => new BinaryNode(a, b, (x, y) => x <= y ? 1 : 0), _precedenceTiers["relational"], "Returns 1 if its left-hand operand is less than or equal to its right-hand operand and otherwise returns 0."),
		new(">=", (a, b, solver) => new BinaryNode(a, b, (x, y) => x >= y ? 1 : 0), _precedenceTiers["relational"], "Returns 1 if its left-hand operand is greater than or equal to its right-hand operand and otherwise returns 0."),

		new("==", (a, b, solver) => new BinaryNode(a, b, (x, y) => x == y ? 1 : 0), _precedenceTiers["equality"], "Returns 1 if its left-hand operand is equal to its right-hand operand and otherwise returns 0."),
		new("!=", (a, b, solver) => new BinaryNode(a, b, (x, y) => x != y ? 1 : 0), _precedenceTiers["equality"], "Returns 1 if its left-hand operand is not equal to its right-hand operand, and otherwise returns 0."),

		new("&", (a, b, solver) => new BinaryNode(a, b, (x, y) => (int)x & (int)y), _precedenceTiers["bitwise_and"], "Converts its operands to integers and computes their bitwise and."),

		new("^", (a, b, solver) => new BinaryNode(a, b, (x, y) => (int)x ^ (int)y), _precedenceTiers["bitwise_xor"], "Converts its operands to integers and computes their bitwise xor."),

		new("|", (a, b, solver) => new BinaryNode(a, b, (x, y) => (int)x | (int)y), _precedenceTiers["bitwise_or"], "Converts its operands to integers and computes their bitwise or."),

		new("&&", (a, b, solver) => new AndNode(a, b), _precedenceTiers["conditional_and"], "Evaluates its left-hand operand. It then returns 0 if its left-hand operand equals 0. Otherwise, it evaluates its right-hand operand. If either operand evaluates to 0, it returns 0. Otherwise, it returns 1."),

		new("||", (a, b, solver) => new OrNode(a, b), _precedenceTiers["conditional_or"], "Evaluates its left-hand operand. It then returns 1 if its left-hand operand equals 1. Otherwise, it evaluates its right-hand operand. If both operands evaluate to 0, it returns 0. Otherwise, it returns 1."),

		new("?:", (a, b, solver) => new ElvisNode(a, b), _precedenceTiers["elvis"], "Evaluates its left-hand operand. It then returns its left-hand operand if it does not evaluate to 0. Otherwise, it evaluates and returns its right-hand operand."),

		new("**=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("**"), b)), _precedenceTiers["assignment"], "Computes the ** of its operands, and assigns that value to its left-hand operand and returns it."),
		new("*=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("*"), b)), _precedenceTiers["assignment"], "Computes the * of its operands, and assigns that value to its left-hand operand and returns it."),
		new("/=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("/"), b)), _precedenceTiers["assignment"], "Computes the / of its operands, and assigns that value to its left-hand operand and returns it."),
		new("%=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("%"), b)), _precedenceTiers["assignment"], "Computes the % of its operands, and assigns that value to its left-hand operand and returns it."),
		new("//=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("//"), b)), _precedenceTiers["assignment"], "Computes the // of its operands, and assigns that value to its left-hand operand and returns it."),
		new("+=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("+"), b)), _precedenceTiers["assignment"], "Computes the += of its operands, and assigns that value to its left-hand operand and returns it."),
		new("-=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("-"), b)), _precedenceTiers["assignment"], "Computes the -= of its operands, and assigns that value to its left-hand operand and returns it."),
		new("<<=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("<<"), b)), _precedenceTiers["assignment"], "Computes the <<= of its operands, and assigns that value to its left-hand operand and returns it."),
		new(">>=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new(">>"), b)), _precedenceTiers["assignment"], "Computes the >>= of its operands, and assigns that value to its left-hand operand and returns it."),
		new("&=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("&"), b)), _precedenceTiers["assignment"], "Computes the &= of its operands, and assigns that value to its left-hand operand and returns it."),
		new("^=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("^"), b)), _precedenceTiers["assignment"], "Computes the ^= of its operands, and assigns that value to its left-hand operand and returns it."),
		new("|=", (a, b, solver) => new AssignmentNode((IAssignable)a, CreateNode(a, new("|"), b)), _precedenceTiers["assignment"], "Computes the |= of its operands, and assigns that value to its left-hand operand and returns it."),

		new("?=", (a, b, solver) => new ElvisAssignmentNode((IAssignable)a, b), _precedenceTiers["assignment"], "Computes the ?: of its operands, and assigns that value to its left-hand operand and returns it."),
		new("=", (a, b, solver) => new AssignmentNode((IAssignable)a, b), _precedenceTiers["assignment"], "Assigns its right-hand operand to its left-hand operand."),

		new(":", (a, b, solver) => new PrintNode(a, b, solver, false), _precedenceTiers["print"], PrintNode.Documentation),

		new("::", (a, b, solver) => new PrintNode(a, b, solver, true), _precedenceTiers["print"], "Acts like the ':' operator, but also prints a newline at the end."),

		new(";", (a, b, solver) => new BinaryNode(a, b, (x, y) => y), _precedenceTiers["semicolon"], "Evaluates both operands and returns its right-hand operand."),

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

	record struct Operator(string Symbol, Func<INode, INode, Solver, INode> Construct, int Precedence, string Prompt);

	public static Associativity GetTierAssociativity(int tier) => tier == _precedenceTiers["assignment"] || tier == _precedenceTiers["elvis"] || tier == _precedenceTiers["exponential"] ? Associativity.Right : Associativity.Left;

	public enum Associativity { Left, Right }
}
