namespace WingCalculatorShared;

internal record VariableNode(string Name, Solver Solver) : INode, IAssignable
{
	public double Solve() => Solver.GetVariable(Name);

	public void Assign(INode a) => Solver.SetVariable(Name, a.Solve());
}
