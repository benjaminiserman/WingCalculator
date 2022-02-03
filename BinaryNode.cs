namespace Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal record BinaryNode(INode A, INode B, Func<double, double, double> Func) : INode
{
	public double Solve() => Func(A.Solve(), B.Solve());
}
