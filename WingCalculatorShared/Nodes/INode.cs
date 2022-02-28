namespace WingCalculatorShared;

internal interface INode
{
	double Solve(Scope scope);

	INode GetAssign(Scope scope) => this;
}
