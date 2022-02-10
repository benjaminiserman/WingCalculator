namespace WingCalculatorShared.Nodes;

internal record AssignmentNode(IAssignable A, INode B) : INode
{
	public double Solve() => A.Assign(B);
}
