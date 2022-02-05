namespace WingCalculatorShared;
using System;

internal record BinaryNode(INode A, INode B, Func<double, double, double> Func) : INode
{
	public double Solve() => Func(A.Solve(), B.Solve());
}
