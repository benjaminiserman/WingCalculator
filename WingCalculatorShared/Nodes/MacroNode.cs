namespace WingCalculatorShared.Nodes;
using WingCalculatorShared.Exceptions;

internal record MacroNode(string Name, Solver Solver, LocalList LocalList, bool Assignable) : INode, IAssignable
{
	public double Solve() => Solver.GetMacro(Name).Solve();

	public double Assign(INode a)
	{
		if (Assignable) return Solver.SetMacro(Name, a);
		else throw new WingCalcException("Macros with arguments cannot be set.");
	}

	public double Assign(double a)
	{
		if (Assignable) return Solver.SetMacro(Name, new ConstantNode(a, Solver));
		else throw new WingCalcException("Macros with arguments cannot be set.");
	}
}
