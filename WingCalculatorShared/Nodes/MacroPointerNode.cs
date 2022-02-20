namespace WingCalculatorShared.Nodes;

internal record MacroPointerNode(INode A, Solver Solver) : INode, IAssignable
{
	public double Solve() => Solver.GetMacro(A.Solve().ToString()).Solve();

	public double Assign(INode b) => Solver.SetMacro(A.Solve().ToString(), b);

	public double Assign(double b) => Solver.SetMacro(A.Solve().ToString(), new ConstantNode(b, Solver));
}
