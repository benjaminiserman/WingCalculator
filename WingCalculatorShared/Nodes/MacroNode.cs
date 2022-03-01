namespace WingCalculatorShared.Nodes;
using WingCalculatorShared.Exceptions;

internal record MacroNode(string Name, LocalList LocalList, bool Assignable) : INode, IAssignable, ICallable
{
	public double Solve(Scope scope) => scope.Solver.GetMacro(Name).Solve(new(LocalList, scope, scope.Solver, Name));

	public double Assign(INode a, Scope scope)
	{
		if (Assignable) return scope.Solver.SetMacro(Name, a);
		else throw new WingCalcException("Macros with arguments cannot be assigned to.", scope);
	}

	public double DeepAssign(INode a, Scope scope)
	{
		INode node = scope.Solver.GetMacro(Name);

		if (node is IAssignable ia) return ia.DeepAssign(a, scope);
		else return Assign(a.GetAssign(scope), scope);
	}

	public INode GetAssign(Scope scope) => scope.Solver.GetMacro(Name);

	public double Call(Scope scope, LocalList list) => scope.Solver.GetMacro(Name).Solve(new(list, scope, scope.Solver, Name));
}
