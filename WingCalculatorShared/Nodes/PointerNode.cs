namespace WingCalculatorShared.Nodes;

internal record PointerNode(INode A, Solver Solver) : INode, IAssignable
{
	public double Address => A.Solve();

	public double Solve() => Solver.GetVariable(A.Solve().ToString());

	public double Assign(INode b) => Solver.SetVariable(A.Solve().ToString(), b.Solve());

	public double Assign(double b) => Solver.SetVariable(A.Solve().ToString(), b);
}
