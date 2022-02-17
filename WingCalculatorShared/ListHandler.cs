namespace WingCalculatorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using WingCalculatorShared.Nodes;

internal static class ListHandler
{
	public static void Allocate(PointerNode pointer, List<double> values)
	{
		Solver solver = pointer.Solver;
		double address = pointer.Address;

		solver.SetVariable(address.ToString(), values.Count);

		for (int i = 1; i <= values.Count; i++)
		{
			solver.SetVariable((address + i).ToString(), values[i - 1]);
		}
	}

	public static double Length(PointerNode pointer) => pointer.Solver.GetVariable(pointer.Address.ToString());

	public static double Get(PointerNode pointer, double i) => pointer.Solver.GetVariable((pointer.Address + i + 1).ToString());

	public static IEnumerable<double> Enumerate(PointerNode pointer)
	{
		Solver solver = pointer.Solver;
		double address = pointer.Address;

		double length = solver.GetVariable(address.ToString());

		for (int i = 1; i <= length; i++)
		{
			yield return solver.GetVariable((address + i).ToString());
		}
	}

	public static double Median(this IEnumerable<double> list)
	{
		List<double> sorted = list.ToList();
		sorted.Sort();

		return sorted.Count % 2 == 0
			? sorted.GetRange(sorted.Count / 2 - 1, 2).Average()
			: sorted[sorted.Count / 2];
	}

	public static double Mode(this IEnumerable<double> list) => list.GroupBy(v => v).OrderByDescending(g => g.Count()).First().Key;

	public static double Product(this IEnumerable<double> list) => list.Aggregate((a, b) => a * b);

	public static double Solve(List<INode> args, Func<IEnumerable<double>, double> func)
	{
		if (args[0] is PointerNode pointer) return func(Enumerate(pointer));
		else return func(args.Select(x => x.Solve()));
	}
}
