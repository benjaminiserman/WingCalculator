namespace WingCalculatorShared.Nodes;
using System;
using System.Collections.Generic;
using WingCalculatorShared.Exceptions;

internal record FunctionNode(string Name, Solver Solver, List<INode> Nodes) : INode
{
	public double Solve()
	{
		try
		{
			return Functions.Get(Name)(Nodes);
		}
		catch (ArgumentOutOfRangeException)
		{
			throw new WingCalcException($"Function \"{Name}\" expects more than {Nodes.Count} parameters.");
		}
		catch (NullReferenceException)
		{
			throw new WingCalcException($"Function \"{Name}\" cannot be called without parameters.");
		}
		catch (InvalidCastException)
		{
			throw new WingCalcException($"Function \"{Name}\" was unable to cast some parameter(s).");
		}
		catch (KeyNotFoundException)
		{
			throw new WingCalcException($"Function \"{Name}\" does not exist.");
		}
	}
}
