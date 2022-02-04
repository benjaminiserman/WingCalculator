namespace WingCalculatorShared;

internal record QuoteNode(string Text, Solver Solver) : INode
{
	public double Solve()
	{
		Solver.Write(Text);
		return Text.Length;
	}
}