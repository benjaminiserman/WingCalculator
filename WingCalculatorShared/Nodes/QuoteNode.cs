namespace WingCalculatorShared.Nodes;

internal record QuoteNode(string Text, Solver Solver) : INode
{
	public double Solve() => Text.Length;
}