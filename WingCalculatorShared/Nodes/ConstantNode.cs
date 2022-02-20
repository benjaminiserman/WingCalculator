namespace WingCalculatorShared.Nodes;

internal record ConstantNode(double Value, Solver Solver) : INode
{
	public double Solve() => Value;
}
