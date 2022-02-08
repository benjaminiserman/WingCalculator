namespace WingCalculatorShared;

internal record LocalNode(INode A, LocalList Locals) : INode, IAssignable
{
	public double Solve() => Locals[(int)A.Solve()].Solve();

	public double Assign(INode a)
	{
		Locals[(int)A.Solve()] = a;
		return 1;
	}

	public double Assign(double a)
	{
		Locals[(int)A.Solve()] = new ConstantNode(a);
		return 1;
	}
}

