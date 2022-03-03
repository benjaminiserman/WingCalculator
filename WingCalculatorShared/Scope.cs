namespace WingCalculatorShared;

internal record Scope(LocalList LocalList, Scope ParentScope, Solver Solver, string Name)
{
	public string Trace()
	{
		if (ParentScope == null) return Name;
		else return $"{ParentScope.Trace()} -> {Name}";
	}
}