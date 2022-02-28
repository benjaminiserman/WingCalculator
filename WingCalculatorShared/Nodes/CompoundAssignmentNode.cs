namespace WingCalculatorShared.Nodes;

internal record CompoundAssignmentNode(IAssignable A, string Operator, INode B) : INode
{
	public double Solve(Scope scope)
	{
		INode a = A;
		if (a is ILocal local) a = local.GetNonLocal(scope);

		if (a is IAssignable ia)
		{
			if (a is ICallable)
			{
				a = a.GetAssign(scope);
				return ia.Assign(Operators.CreateNode(a, new PreOperatorNode(Operator), B), scope);
			}
			else
			{
				return ia.Assign(Operators.GetOperatorFunction(Operator)(a.Solve(scope), B.Solve(scope)), scope);
			}
		}
		else return A.Assign(Operators.GetOperatorFunction(Operator)(a.Solve(scope), B.Solve(scope)), scope);
	}
}
