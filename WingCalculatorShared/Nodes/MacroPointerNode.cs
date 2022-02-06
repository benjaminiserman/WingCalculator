namespace WingCalculatorShared;

internal record MacroPointerNode(INode A, Solver Solver) : INode, IAssignable
{
	public double Solve() => Solver.GetMacro(A.Solve().ToString()).Solve();

	public void Assign(INode b) => Solver.SetMacro(A.Solve().ToString(), b);

	public void Assign(double b) => Solver.SetMacro(A.Solve().ToString(), new ConstantNode(b));
}
