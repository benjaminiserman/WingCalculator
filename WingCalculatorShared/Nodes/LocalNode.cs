namespace WingCalculatorShared.Nodes;

internal record LocalNode(INode A, Solver Solver) : INode, IAssignable
{
	public double Solve() => Solve(0);

	private double Solve(int i)
	{
		INode node;

		try
		{
			node = Solver.PeekCallStack(i)[A.Solve().ToString()];
		}
		catch
		{
			throw new Exceptions.WingCalcException($"solve {i}: {A}");
		}

		if (node is LocalNode local) return local.Solve(i + 1);
		else return node.Solve();
	}

	public double Assign(INode b) => Assign(b, 0);

	public double Assign(INode b, int i)
	{
		INode node;
		try
		{
			node = Solver.PeekCallStack(i)[A.Solve().ToString()];
		}
		catch
		{
			throw new Exceptions.WingCalcException($"set {i}: {A}; {b}");
		}

		try
		{
			if (node is LocalNode local) return local.Assign(b, i + 1);
			else if (node is IAssignable ia) return ia.Assign(b);
			else
			{
				Solver.PeekCallStack()[A.Solve().ToString()] = b;
				return 1;
			}
		}
		catch
		{
			throw new Exceptions.WingCalcException($"{i}: {node} set to {b}");
		}
	}

	public double Assign(double b) => Assign(b, 0);
	public double Assign(double b, int i)
	{
		INode node;
		try
		{
			node = Solver.PeekCallStack(i)[A.Solve().ToString()];
		}
		catch
		{
			throw new Exceptions.WingCalcException($"set {i}: {A}; {b}");
		}

		try
		{
			if (node is LocalNode local) return local.Assign(b, i + 1);
			else if (node is IAssignable ia) return ia.Assign(b);
			else
			{
				Solver.PeekCallStack()[A.Solve().ToString()] = new ConstantNode(b, Solver);
				return 1;
			}
		}
		catch
		{
			throw new Exceptions.WingCalcException($"{i}: {node} set to {b}");
		}
	}
}

