namespace WingCalculatorShared;

internal record VariableNode(string Name, Solver Solver) : INode, IAssignable
{
	public double Solve() => Solver.GetVariable(Name);

	public double Assign(INode a) => Solver.SetVariable(Name, a.Solve());

	public double Assign(double a) => Solver.SetVariable(Name, a);
}
