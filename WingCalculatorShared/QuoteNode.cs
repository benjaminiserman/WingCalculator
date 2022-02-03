namespace WingCalculatorShared;

internal record QuoteNode(string Text) : INode
{
	public double Solve()
	{
		Console.Write(Text);
		return Text.Length;
	}
}