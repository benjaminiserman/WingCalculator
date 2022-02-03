namespace WingCalculatorShared;

internal record PointerNode(INode A, Solver Solver) : INode, IAssignable
{
	public double Solve() => Solver.GetVariable(A.Solve().ToString());

	public void Assign(INode b) => Solver.SetVariable(A.Solve().ToString(), b.Solve());
}
