namespace WingCalculatorShared.Nodes;

internal interface IAssignable : INode
{
	double Assign(INode a, Scope scope);
	double Assign(double a, Scope scope) => Assign(new ConstantNode(a), scope);
	double DeepAssign(INode a, Scope scope) => Assign(a.GetAssign(scope), scope);
	double DeepAssign(double a, Scope scope) => DeepAssign(new ConstantNode(a), scope);
}
