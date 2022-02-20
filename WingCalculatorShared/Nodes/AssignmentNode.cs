namespace WingCalculatorShared.Nodes;

internal record AssignmentNode(IAssignable A, INode B, Solver Solver) : INode
{
	public double Solve() => A.Assign(B);
}
