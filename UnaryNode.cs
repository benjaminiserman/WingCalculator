namespace WingCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal record UnaryNode(INode A, Func<double, double> Func) : INode
{
	public double Solve() => Func(A.Solve());
}
