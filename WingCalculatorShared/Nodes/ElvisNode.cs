namespace WingCalculatorShared.Nodes;

internal record ElvisNode(INode A, INode B, Solver Solver) : INode
{
	public double Solve()
	{
		double a = A.Solve();

		if (a != 0) return a;
		else return B.Solve();
	}
}