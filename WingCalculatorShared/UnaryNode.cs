namespace WingCalculatorShared;
using System;

internal record UnaryNode(INode A, Func<double, double> Func) : INode
{
	public double Solve() => Func(A.Solve());
}
