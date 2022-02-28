namespace WingCalculatorShared.Nodes;

using System.Net;
using WingCalculatorShared.Exceptions;

internal record LocalNode(string Name) : INode, IAssignable, IPointer, ILocal, ICallable
{
	public double Solve(Scope scope) => scope.LocalList[Name].Solve(scope.ParentScope);

	public string GetName(Scope scope) => Name;

	public double Assign(INode b, Scope scope)
	{
		if (b is ILocal local) b = local.GetNonLocal(scope);

		scope.LocalList[Name] = b;
		return 1;
	}

	public double DeepAssign(INode b, Scope scope)
	{
		string address = Name;
		INode a = scope.LocalList[address];
		if (b is ILocal local) b = local.GetNonLocal(scope);

		if (a is IAssignable ia) return ia.DeepAssign(b, a is ILocal ? scope.ParentScope : scope);
		else
		{
			scope.LocalList[address] = b;
			return 1;
		}
	}

	public double Address(Scope scope)
	{
		INode node = scope.LocalList[Name];

		if (node is IPointer pointer and not ILocal) return pointer.Address(scope);
		else throw new WingCalcException($"#{Name} could not be interpreted as a pointer.");
	}

	public double Set(string address, INode a, Scope scope)
	{
		string myAddress = Address(scope).ToString();

		if (scope.LocalList.Contains(myAddress))
		{
			INode node = scope.LocalList[myAddress];

			if (node is IPointer pointer and not ILocal) return pointer.Set(address, a, scope);
			else throw new WingCalcException($"#{Name} could not be interpreted as a pointer.");
		}
		else throw new WingCalcException($"#{Name} could not be interpreted as a pointer.");
	}

	public double Get(string address, Scope scope)
	{
		INode node = scope.LocalList[Address(scope)];

		if (node is IPointer pointer and not ILocal) return pointer.Get(address, scope);
		else throw new WingCalcException($"#{Name} could not be interpreted as a pointer.");
	}

	public INode GetAssign(Scope scope) => scope.LocalList[GetName(scope)];

	public double Call(Scope scope, LocalList list)
	{
		INode node = scope.LocalList[Address(scope)];

		if (node is ICallable callable and not ILocal) return callable.Call(scope, list);
		else throw new WingCalcException($"#{Name} could not be interpreted as callable.");
	}
}
