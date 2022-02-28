namespace WingCalculatorShared.Nodes;

internal interface ILocal : IAssignable, INode
{
	string GetName(Scope scope);

	(INode, Scope) GetNonLocal(Scope scope)
	{
		INode node = scope.LocalList[GetName(scope)];

		if (node is ILocal gotLocal) return gotLocal.GetNonLocal(scope.ParentScope);
		else return (node, scope);
	}
}