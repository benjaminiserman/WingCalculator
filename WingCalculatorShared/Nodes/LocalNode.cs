namespace WingCalculatorShared.Nodes;

internal record LocalNode(INode A, Solver Solver) : INode, IAssignable
{
	public double Solve() => GetNonLocal(A).node.Solve();

	public double Assign(INode b)
	{
		if (b is LocalNode gotLocal) b = GetNonLocal(gotLocal.A).node;

		Solver.PeekCallStack(0)[A.Solve().ToString()] = b;
		return 1;
	}

	public double Assign(double b) => Assign(new ConstantNode(b, Solver));

	private (INode node, int i) GetNonLocal(INode a, int i = 0)
	{
		INode node = Solver.PeekCallStack(i)[a.Solve().ToString()];

		if (node is LocalNode gotLocal) return GetNonLocal(gotLocal.A, i + 1);
		else return (node, i);
	}

	public double SetLocal(INode b, int i = 0)
	{
		if (b is LocalNode gotLocal) b = GetNonLocal(gotLocal.A).node;

		if (A is LocalNode local) return local.SetLocal(b, i + 1);
		else
		{
			string address = A.Solve().ToString();
			INode node = Solver.PeekCallStack(i)[address];

			if (node is IAssignable ia) return ia.Assign(b);
			else
			{
				Solver.PeekCallStack(i)[address] = b;
				return 1;
			}
		}
	}
}
