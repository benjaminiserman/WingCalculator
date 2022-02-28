namespace WingCalculatorShared.Nodes;

internal record ElvisNode(INode A, INode B) : INode
{
	public double Solve(Scope scope)
	{
		double a = A.Solve(scope);

		if (a != 0) return a;
		else return B.Solve(scope);
	}
}