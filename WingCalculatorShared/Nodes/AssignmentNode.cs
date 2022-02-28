namespace WingCalculatorShared.Nodes;

internal record AssignmentNode(IAssignable A, INode B) : INode
{
	public double Solve(Scope scope) => A.Assign(B, scope);
}
