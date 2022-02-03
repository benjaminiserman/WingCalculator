namespace WingCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal record PointerNode(INode A, Solver Solver) : INode, IAssignable
{
	public double Solve() => Solver.GetVariable(A.Solve().ToString());

	public void Assign(INode b) => Solver.SetVariable(A.Solve().ToString(), b.Solve());
}
