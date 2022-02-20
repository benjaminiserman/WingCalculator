namespace WingCalculatorShared.Nodes;
using System;

internal class PreOperatorNode : INode
{
	public static readonly int MaxTier = 15;

	public string Text { get; private set; }

	public PreOperatorNode(string s) => Text = s;

	public double Solve() => throw new NotImplementedException("Syntax error. PreOperatorNode survived to Solve phase.");

	public int Tier => Operators.GetPrecedence(Text);

	public Solver Solver => throw new NotImplementedException("Syntax error. PreOperatorNode survived to Solve phase.");
}
