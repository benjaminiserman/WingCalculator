namespace WingCalculatorShared.Nodes;
using System;
using System.Collections.Generic;
using WingCalculatorShared.Exceptions;

internal record FunctionNode(string Name, LocalList Locals) : INode
{
	public double Solve(Scope scope)
	{
		//scope.Solver.PushCallStack(Locals);
		List<INode> Nodes = (List<INode>)Locals;

		try
		{
			return Functions.Get(Name)(Nodes, scope);
		}
		catch (ArgumentOutOfRangeException)
		{
			throw new WingCalcException($"Function \"{Name}\" expects more than {Nodes.Count} parameters.", scope);
		}
		catch (NullReferenceException)
		{
			throw new WingCalcException($"Function \"{Name}\" cannot be called without parameters.", scope);
		}
		catch (InvalidCastException)
		{
			throw new WingCalcException($"Function \"{Name}\" was unable to cast some parameters.", scope);
		}
		catch (KeyNotFoundException)
		{
			throw new WingCalcException($"Function \"{Name}\" does not exist.", scope);
		}
		finally
		{
			//scope.Solver.PopCallStack();
		}
	}
}
