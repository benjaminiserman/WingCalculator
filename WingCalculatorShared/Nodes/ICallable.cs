namespace WingCalculatorShared.Nodes;

internal interface ICallable : INode
{
	double Call(Scope scope, LocalList list);
}