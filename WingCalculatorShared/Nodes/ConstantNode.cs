namespace WingCalculatorShared;

internal record ConstantNode(double Value) : INode
{
	public double Solve() => Value;
}
