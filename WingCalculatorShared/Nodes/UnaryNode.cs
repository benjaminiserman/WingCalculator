namespace WingCalculatorShared.Nodes;
using System;

internal record UnaryNode(INode A, Func<double, double> Func, Solver Solver) : INode
{
	public double Solve() => Func(A.Solve());
}
