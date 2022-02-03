namespace WingCalculatorShared;

internal record MacroNode(string Name, Solver Solver) : INode, IAssignable
{
	public double Solve() => Solver.GetMacro(Name).Solve();

	public void Assign(INode a) => Solver.SetMacro(Name, a);
}
