namespace WingCalculatorShared;

internal record AssignmentNode(IAssignable A, INode B) : INode
{
	public double Solve()
	{
		A.Assign(B);

		if (A is MacroNode or MacroPointerNode) return 1;
		else return ((INode)A).Solve(); // probably shouldn't double-dip
	}
}
