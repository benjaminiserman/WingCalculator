namespace WingCalculatorShared.Nodes;

internal record VariableNode(string Name) : INode, IAssignable
{
	public double Solve(Scope scope) => scope.Solver.GetVariable(Name);

	public double Assign(INode a, Scope scope) => scope.Solver.SetVariable(Name, a.Solve(scope));

	public double Assign(double a, Scope scope) => scope.Solver.SetVariable(Name, a);
}
