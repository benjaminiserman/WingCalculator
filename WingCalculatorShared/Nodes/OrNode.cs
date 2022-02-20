namespace WingCalculatorShared.Nodes;

internal record OrNode(INode A, INode B, Solver Solver) : INode
{
	public double Solve() => A.Solve() != 0 || B.Solve() != 0 ? 1 : 0;
}
