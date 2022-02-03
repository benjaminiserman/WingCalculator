namespace Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal record AssignmentNode(VariableNode A, INode B) : INode
{
	public double Solve()
	{
		double x = B.Solve();
		A.Solver.SetVariable(A.Name, x);

		return x;
	}

	public static explicit operator VariableNode(AssignmentNode a) => a.A;
}
