namespace WingCalculatorShared;
using System.Collections.Generic;
using System.Linq;

internal record FunctionNode(string Name, Solver Solver, List<INode> Nodes) : INode
{
	public double Solve() => Solver.GetFunction(Name)((from x in Nodes select x.Solve()).ToList());
}

