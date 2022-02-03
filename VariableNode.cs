namespace WingCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal record VariableNode(string Name, Solver Solver) : INode, IAssignable
{
	public double Solve() => Solver.GetVariable(Name);

	public void Assign(INode a) => Solver.SetVariable(Name, a.Solve());
}
