namespace WingCalculatorShared.Nodes;
using WingCalculatorShared.Exceptions;

internal record MacroNode(string Name, LocalList LocalList, bool Assignable) : INode, IAssignable
{
	public double Solve(Scope scope) => scope.Solver.GetMacro(Name).Solve(new(LocalList, scope, scope.Solver));

	public double Assign(INode a, Scope scope)
	{
		if (Assignable) return scope.Solver.SetMacro(Name, a);
		else throw new WingCalcException("Macros with arguments cannot be set.");
	}
}
