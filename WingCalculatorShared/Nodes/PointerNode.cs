namespace WingCalculatorShared.Nodes;

internal record PointerNode(INode A) : INode, IAssignable, IPointer
{
	public double Solve(Scope scope) => scope.Solver.GetVariable(A.Solve(scope).ToString());

	public double Assign(INode b, Scope scope) => scope.Solver.SetVariable(A.Solve(scope).ToString(), b.Solve(scope));

	public double Assign(double b, Scope scope) => scope.Solver.SetVariable(A.Solve(scope).ToString(), b);

	public double Address(Scope scope) => A.Solve(scope);
	public double Set(string address, double x, Scope scope) => scope.Solver.SetVariable(address, x);
	public double Set(string address, INode a, Scope scope) => Set(address, a.Solve(scope), scope);
	public double Get(string address, Scope scope) => scope.Solver.GetVariable(address);
}
