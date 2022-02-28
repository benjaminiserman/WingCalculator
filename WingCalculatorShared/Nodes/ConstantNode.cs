namespace WingCalculatorShared.Nodes;

internal record ConstantNode(double Value) : INode
{
	public double Solve(Scope scope) => Value;
}
