namespace WingCalculatorShared;
using System;

internal static class OperatorNodeFactory
{
	public static INode CreateBinaryNode(INode a, PreOperatorNode op, INode b) => op.Text switch
	{
		"?" => new PromptNode((IAssignable)a, b),

		"**" => new BinaryNode(a, b, (x, y) => Math.Pow(x, y)),

		"*" => new BinaryNode(a, b, (x, y) => x * y),
		"/" => new BinaryNode(a, b, (x, y) => x / y),
		"%" => new BinaryNode(a, b, (x, y) => x % y),
		"//" => new BinaryNode(a, b, (x, y) => Math.Floor(x / y)),

		"+" => new BinaryNode(a, b, (x, y) => x + y),
		"-" => new BinaryNode(a, b, (x, y) => x - y),

		"<<" => new BinaryNode(a, b, (x, y) => (int)x << (int)y),
		">>" => new BinaryNode(a, b, (x, y) => (int)x >> (int)y),

		"<" => new BinaryNode(a, b, (x, y) => x < y ? 1 : 0),
		">" => new BinaryNode(a, b, (x, y) => x > y ? 1 : 0),
		"<=" => new BinaryNode(a, b, (x, y) => x <= y ? 1 : 0),
		">=" => new BinaryNode(a, b, (x, y) => x >= y ? 1 : 0),

		"<?" => new BinaryNode(a, b, (x, y) => x < y ? x : y),
		">?" => new BinaryNode(a, b, (x, y) => x > y ? x : y),
		"<=?" => new BinaryNode(a, b, (x, y) => x <= y ? x : y),
		">=?" => new BinaryNode(a, b, (x, y) => x >= y ? x : y),

		"==" => new BinaryNode(a, b, (x, y) => x == y ? 1 : 0),
		"!=" => new BinaryNode(a, b, (x, y) => x != y ? 1 : 0),

		"&" => new BinaryNode(a, b, (x, y) => (int)x & (int)y),

		"^" => new BinaryNode(a, b, (x, y) => (int)x ^ (int)y),

		"|" => new BinaryNode(a, b, (x, y) => (int)x | (int)y),

		"&&" => new AndNode(a, b),

		"||" => new OrNode(a, b),

		"?:" => new ElvisNode(a, b),

		"*=" => new AssignmentNode((IAssignable)a, CreateBinaryNode(a, new("*"), b)),

		//"*=" => new BinaryNode(a, b, (x, y) => ((VariableNode)a).Solver.SetVariable(((VariableNode)a).Name, x * y)),
		"/=" => new BinaryNode(a, b, (x, y) => ((VariableNode)a).Solver.SetVariable(((VariableNode)a).Name, x / y)),
		"%=" => new BinaryNode(a, b, (x, y) => ((VariableNode)a).Solver.SetVariable(((VariableNode)a).Name, x % y)),
		"//=" => new BinaryNode(a, b, (x, y) => ((VariableNode)a).Solver.SetVariable(((VariableNode)a).Name, Math.Floor(x / y))),
		"+=" => new BinaryNode(a, b, (x, y) => ((VariableNode)a).Solver.SetVariable(((VariableNode)a).Name, x + y)),
		"-=" => new BinaryNode(a, b, (x, y) => ((VariableNode)a).Solver.SetVariable(((VariableNode)a).Name, x - y)),
		"<<=" => new BinaryNode(a, b, (x, y) => ((VariableNode)a).Solver.SetVariable(((VariableNode)a).Name, (int)x << (int)y)),
		">>=" => new BinaryNode(a, b, (x, y) => ((VariableNode)a).Solver.SetVariable(((VariableNode)a).Name, (int)x >> (int)y)),
		"&=" => new BinaryNode(a, b, (x, y) => ((VariableNode)a).Solver.SetVariable(((VariableNode)a).Name, (int)x & (int)y)),
		"^=" => new BinaryNode(a, b, (x, y) => ((VariableNode)a).Solver.SetVariable(((VariableNode)a).Name, (int)x ^ (int)y)),
		"|=" => new BinaryNode(a, b, (x, y) => ((VariableNode)a).Solver.SetVariable(((VariableNode)a).Name, (int)x | (int)y)),
		"?=" => new ElvisAssignmentNode((IAssignable)a, b),
		"=" => new AssignmentNode((IAssignable)a, b),

		":" => new BinaryNode(a, b, (x, y) =>
		{
			Console.Write(y switch
			{
				-1 => '\n',
				0 => x.ToString(),
				1 => (char)(int)x,
				2 => Convert.ToString((int)x, 2),
				8 => Convert.ToString((int)x, 8),
				16 => Convert.ToString((int)x, 16),

				_ => throw new NotImplementedException($"Format {y} is not implemented.")
			});

			return x;
		}),

		";" => new BinaryNode(a, b, (x, y) => y),

		_ => throw new NotImplementedException()
	};
}
