namespace WingCalculatorShared.Nodes;

internal record LambdaNode(INode Node, bool Assignable) : INode, ICallable
{
	public double Solve(Scope scope) => Node.Solve(new(new(), scope, scope.Solver));

	public double Call(Scope scope, LocalList list) => Node.Solve(new(list, scope, scope.Solver));
}
