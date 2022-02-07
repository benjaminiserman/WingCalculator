namespace WingCalculatorShared;

internal record LocalNode(int Index, List<INode> Locals) : INode, IAssignable
{
	public double Solve() => Locals[Index].Solve();

	public double Assign(INode a)
	{
		Locals[Index] = a;
		return 1;
	}

	public double Assign(double a)
	{
		Locals[Index] = new ConstantNode(a);
		return 1;
	}
}

