namespace WingCalculatorShared.Nodes;
using System;

internal record BinaryNode(INode A, INode B, Func<double, double, double> Func) : INode
{
	public double Solve(Scope scope) => Func(A.Solve(scope), B.Solve(scope));
}
