namespace WingCalculatorShared.Nodes;

internal record PointerNode(INode A) : INode, IAssignable
{
	public double Address(Scope scope) => A.Solve(scope);

	public double Solve(Scope scope) => scope.Solver.GetVariable(A.Solve(scope).ToString());

	public double Assign(INode b, Scope scope) => scope.Solver.SetVariable(A.Solve(scope).ToString(), b.Solve(scope));

	public double Assign(double b, Scope scope) => scope.Solver.SetVariable(A.Solve(scope).ToString(), b);
}
