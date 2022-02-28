namespace WingCalculatorShared.Nodes;
using System;

internal record UnaryNode(INode A, Func<double, double> Func) : INode
{
	public double Solve(Scope scope) => Func(A.Solve(scope));
}
