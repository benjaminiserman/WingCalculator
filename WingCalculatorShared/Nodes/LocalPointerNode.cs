namespace WingCalculatorShared.Nodes;

internal record LocalPointerNode(INode A) : INode, IAssignable, IPointer, ILocal
{
	public double Solve(Scope scope) => scope.LocalList[A.Solve(scope).ToString()].Solve(scope.ParentScope);

	public string GetName(Scope scope) => A.Solve(scope).ToString();

	public double Assign(INode b, Scope scope)
	{
		if (b is ILocal local) (b, _) = local.GetNonLocal(scope);

		scope.LocalList[A.Solve(scope).ToString()] = b;
		return 1;
	}

	public double DeepAssign(INode b, Scope scope)
	{
		string address = A.Solve(scope).ToString();
		INode a = scope.LocalList[address];
		if (b is ILocal local) (b, _) = local.GetNonLocal(scope);

		if (a is IAssignable ia) return ia.DeepAssign(b, a is ILocal ? scope.ParentScope : scope);
		else
		{
			scope.LocalList[address] = b;
			return 1;
		}
	}

	public double Address(Scope scope) => A.Solve(scope);
	public double Set(string address, INode a, Scope scope)
	{
		string myAddress = Address(scope).ToString();

		if (scope.LocalList.Contains(myAddress))
		{
			INode node = scope.LocalList[myAddress];

			if (node is IPointer pointer and not ILocal) return pointer.Set(address, a, scope);
			else return scope.LocalList.Set(address, a);
		}
		else return scope.LocalList.Set(address, a);
	}

	public double Get(string address, Scope scope)
	{
		INode node = scope.LocalList[Address(scope)];

		if (node is IPointer pointer and not ILocal) return pointer.Get(address, scope);
		else return scope.LocalList.Get(address).Solve(scope);
	}

	public INode GetAssign(Scope scope) => scope.LocalList[GetName(scope)];
}
