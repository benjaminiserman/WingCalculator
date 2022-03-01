namespace WingCalculatorShared.Nodes;
using WingCalculatorShared.Exceptions;

internal record LocalPointerNode(INode A) : INode, IAssignable, IPointer, ILocal, ICallable
{
	public double Solve(Scope scope) => scope.LocalList[A.Solve(scope).ToString(), scope].Solve(scope.ParentScope);

	public string GetName(Scope scope) => A.Solve(scope).ToString();

	public double Assign(INode b, Scope scope)
	{
		if (b is ILocal local) b = local.GetNonLocal(scope);

		scope.LocalList[A.Solve(scope).ToString(), scope] = b;
		return 1;
	}

	public double DeepAssign(INode b, Scope scope)
	{
		string address = A.Solve(scope).ToString();
		INode a = scope.LocalList[address, scope];
		if (b is ILocal local) b = local.GetNonLocal(scope);

		if (a is IAssignable ia) return ia.DeepAssign(b, a is ILocal ? scope.ParentScope : scope);
		else
		{
			scope.LocalList[address, scope] = b;
			return 1;
		}
	}

	public double Address(Scope scope) => A.Solve(scope);
	public double Set(string address, INode a, Scope scope)
	{
		string myAddress = Address(scope).ToString();

		if (scope.LocalList.Contains(myAddress))
		{
			INode node = scope.LocalList[myAddress, scope];

			if (node is IPointer pointer and not ILocal) return pointer.Set(address, a, scope);
			else return scope.LocalList.Set(address, a);
		}
		else return scope.LocalList.Set(address, a);
	}

	public double Get(string address, Scope scope)
	{
		INode node = scope.LocalList[Address(scope), scope];

		if (node is IPointer pointer and not ILocal) return pointer.Get(address, scope);
		else return scope.LocalList[address, scope].Solve(scope);
	}

	public INode GetAssign(Scope scope) => scope.LocalList[GetName(scope), scope];

	public double Call(Scope scope, LocalList list)
	{
		INode node = scope.LocalList[Address(scope), scope];

		if (node is ICallable callable and not ILocal) return callable.Call(scope, list);
		else throw new WingCalcException($"#{GetName(scope)} could not be interpreted as callable.", scope);
	}
}
