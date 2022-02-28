namespace WingCalculatorShared.Nodes;

internal interface IAssignable
{
	double Assign(INode a, Scope scope);
	double Assign(double a, Scope scope) => Assign(new ConstantNode(a), scope);
	double DeepAssign(INode a, Scope scope) => Assign(a, scope);
	double DeepAssign(double a, Scope scope) => DeepAssign(new ConstantNode(a), scope);
}
