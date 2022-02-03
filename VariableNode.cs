namespace Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal record VariableNode(string Name, Solver Solver) : INode
{
	public double Solve() => Solver.GetVariable(Name);
}
