namespace WingCalculatorShared.Nodes;

internal record LocalNode(INode A) : INode, IAssignable
{
	public double Solve(Scope scope) => scope.LocalList[A.Solve(scope).ToString()].Solve(scope.ParentScope);

	public double Assign(INode b, Scope scope)
	{
		if (b is LocalNode local) (b, _) = local.GetNonLocal(scope);

		scope.LocalList[A.Solve(scope).ToString()] = b;
		return 1;
	}

	private (INode, Scope) GetNonLocal(Scope scope)
	{
		INode node = scope.LocalList[A.Solve(scope).ToString()];

		if (node is LocalNode gotLocal) return gotLocal.GetNonLocal(scope.ParentScope);
		else return (node, scope);
	}

	public double DeepAssign(INode b, Scope scope)
	{
		string address = A.Solve(scope).ToString();
		INode a = scope.LocalList[address];
		if (b is LocalNode gotLocal) (b, _) = gotLocal.GetNonLocal(scope);

		if (a is IAssignable ia) return ia.DeepAssign(b, a is LocalNode ? scope.ParentScope : scope);
		else
		{
			scope.LocalList[address] = b;
			return 1;
		}
	}
}
