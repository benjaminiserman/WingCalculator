namespace WingCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal record AssignmentNode(IAssignable A, INode B) : INode
{
	public double Solve()
	{
		A.Assign(B);

		if (A is MacroNode macro) return 1;
		else return ((INode)A).Solve(); // probably shouldn't double-dip
	}
}
