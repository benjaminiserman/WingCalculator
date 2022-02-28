namespace WingCalculatorShared.Nodes;

internal record QuoteNode(string Text) : INode
{
	public double Solve(Scope scope) => Text.Length;
}