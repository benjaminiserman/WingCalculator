namespace WingCalculatorShared.Nodes;

internal record AndNode(INode A, INode B) : INode
{
	public double Solve() => A.Solve() != 0 && B.Solve() != 0 ? 1 : 0;
}
