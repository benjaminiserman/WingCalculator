namespace WingCalculatorShared.Nodes;

internal record MacroNode(string Name, Solver Solver) : INode, IAssignable
{
	public double Solve() => Solver.GetMacro(Name).Solve();

	public double Assign(INode a) => Solver.SetMacro(Name, a);

	public double Assign(double a) => Solver.SetMacro(Name, new ConstantNode(a));
}
