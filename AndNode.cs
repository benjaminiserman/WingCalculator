namespace WingCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal record AndNode(INode A, INode B) : INode
{
	public double Solve() => A.Solve() != 0 && B.Solve() != 0 ? 1 : 0;
}
