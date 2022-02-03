namespace WingCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class OperatorNodeFactory
{
	public static INode CreateBinaryNode(INode a, PreOperatorNode op, INode b) => op.Text switch
	{
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

		"==" => new BinaryNode(a, b, (x, y) => x == y ? 1 : 0),
		"!=" => new BinaryNode(a, b, (x, y) => x != y ? 1 : 0),

		"&" => new BinaryNode(a, b, (x, y) => (int)x & (int)y),

		"^" => new BinaryNode(a, b, (x, y) => (int)x ^ (int)y),

		"|" => new BinaryNode(a, b, (x, y) => (int)x | (int)y),

		"&&" => new BinaryNode(a, b, (x, y) => (x != 0) && (y != 0) ? 1 : 0),

		"||" => new BinaryNode(a, b, (x, y) => (x != 0) || (y != 0) ? 1 : 0),

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
		"=" => new AssignmentNode((IAssignable)a, b),

		";" => new BinaryNode(a, b, (x, y) => y),

		_ => throw new NotImplementedException()
	};
}
