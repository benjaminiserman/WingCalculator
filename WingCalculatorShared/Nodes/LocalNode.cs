namespace WingCalculatorShared.Nodes;

internal record LocalNode(INode A, LocalList Locals, Solver Solver) : INode, IAssignable
{
	public double Solve() => Locals[A.Solve().ToString()].Solve();

	public double Assign(INode b)
	{
		Locals[A.Solve().ToString()] = b;
		return 1;
	}

	public double Assign(double b)
	{
		Locals[A.Solve().ToString()] = new ConstantNode(b, Solver);
		return 1;
	}
}

