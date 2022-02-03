namespace WingCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal record FunctionNode(string Name, Solver Solver, List<INode> Nodes) : INode
{
	public double Solve() => Solver.GetFunction(Name)((from x in Nodes select x.Solve()).ToList());
}

