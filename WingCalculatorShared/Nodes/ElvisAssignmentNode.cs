namespace WingCalculatorShared.Nodes;

internal record ElvisAssignmentNode(IAssignable A, INode B) : INode
{
	public double Solve(Scope scope)
	{
		double a = A.Solve(scope);
		if (a == 0) A.Assign(B.GetAssign(scope), scope);

		if (A is MacroNode) return 1;
		else return a;
	}
}