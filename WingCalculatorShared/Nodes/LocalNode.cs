namespace WingCalculatorShared.Nodes;

internal record LocalNode(INode A, Solver Solver, LocalList LocalList) : INode, IAssignable
{
	public double Solve() => GetNonLocal(A).Solve();

	public double Assign(INode b)
	{
		if (b is LocalNode gotLocal) b = GetNonLocal(gotLocal.A);

		LocalList[A.Solve().ToString()] = b;
		return 1;
	}

	public double Assign(double b) => Assign(new ConstantNode(b, Solver));

	private INode GetNonLocal(INode a)
	{
		INode node = LocalList[a.Solve().ToString()];

		if (node is LocalNode gotLocal) return GetNonLocal(gotLocal.A);
		else return node;
	}

	public double SetLocal(INode b)
	{
		if (b is LocalNode gotLocal) b = GetNonLocal(gotLocal.A);

		if (A is LocalNode local) return local.SetLocal(b);
		else
		{
			string address = A.Solve().ToString();
			INode node = LocalList[address];

			if (node is IAssignable ia) return ia.Assign(b);
			else
			{
				LocalList[address] = b;
				return 1;
			}
		}
	}
}
