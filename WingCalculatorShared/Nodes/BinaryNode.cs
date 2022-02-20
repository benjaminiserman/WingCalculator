namespace WingCalculatorShared.Nodes;
using System;

internal record BinaryNode(INode A, INode B, Func<double, double, double> Func, Solver Solver) : INode
{
	public double Solve() => Func(A.Solve(), B.Solve());
}
