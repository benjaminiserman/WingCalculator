namespace WingCalculatorShared.Nodes;

internal interface IPointer : IAssignable, INode
{
	double Address(Scope scope);

	double Set(string address, INode a, Scope scope);
	double Set(string address, double x, Scope scope) => Set(address, new ConstantNode(x), scope);
	double Set(double address, INode a, Scope scope) => Set(address.ToString(), a, scope);
	double Set(double address, double x, Scope scope) => Set(address.ToString(), x, scope);

	double Get(string address, Scope scope);
	double Get(double address, Scope scope) => Get(address.ToString(), scope);
}