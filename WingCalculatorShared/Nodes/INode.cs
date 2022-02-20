namespace WingCalculatorShared;

internal interface INode
{
	double Solve();

	Solver Solver { get; }
}
