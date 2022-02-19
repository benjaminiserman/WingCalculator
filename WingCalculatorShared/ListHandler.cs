namespace WingCalculatorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using WingCalculatorShared.Nodes;

internal static class ListHandler
{
	public static double Allocate(PointerNode pointer, List<double> values)
	{
		Solver solver = pointer.Solver;
		double address = pointer.Address;

		solver.SetVariable(address.ToString(), values.Count);

		for (int i = 1; i <= values.Count; i++)
		{
			solver.SetVariable((address + i).ToString(), values[i - 1]);
		}

		return address;
	}

	public static double StringAllocate(PointerNode pointer, List<double> values)
	{
		Solver solver = pointer.Solver;
		double address = pointer.Address;

		for (int i = 0; i < values.Count; i++)
		{
			solver.SetVariable((address + i).ToString(), values[i]);
		}

		solver.SetVariable((address + values.Count).ToString(), 0);

		return address;
	}

	public static double Length(PointerNode pointer) => pointer.Solver.GetVariable(pointer.Address.ToString());

	public static double Get(PointerNode pointer, double i)
	{
		if (i < 0) i = Length(pointer) + i;
		return pointer.Solver.GetVariable((pointer.Address + i + 1).ToString());
	}

	public static double Set(PointerNode pointer, double i, double x)
	{
		if (i < 0) i = Length(pointer) + i;
		return pointer.Solver.SetVariable((pointer.Address + i + 1).ToString(), x);
	}

	public static double Add(PointerNode pointer, double x)
	{
		Solver solver = pointer.Solver;
		double address = pointer.Address;

		double length = solver.GetVariable(address.ToString()) + 1;
		solver.SetVariable(address.ToString(), length);
		solver.SetVariable((address + length).ToString(), x);

		return x;
	}

	public static double IndexOf(PointerNode pointer, double x)
	{
		Solver solver = pointer.Solver;
		double address = pointer.Address;

		try
		{
			return Enumerate(pointer).Select((x, i) => (x, i)).First(y => y.x == x).i;
		}
		catch
		{
			return -1;
		}
	}

	public static double Remove(PointerNode pointer, double x)
	{
		var list = Enumerate(pointer).ToList();
		if (!list.Contains(x)) return 0;
		else
		{
			list.Remove(x);
			Allocate(pointer, list);
			return 1;
		}
	}

	public static double Clear(PointerNode pointer) => Allocate(pointer, new());

	public static double Concat(PointerNode a, PointerNode b, PointerNode c) => Allocate(c, Enumerate(a).Concat(Enumerate(b)).ToList());

	public static double Copy(PointerNode a, PointerNode b) => Allocate(b, Enumerate(a).ToList());

	public static double Setify(PointerNode a) => Allocate(a, Enumerate(a).ToHashSet().ToList());

	public static double Listify(PointerNode a) => Allocate(a, StringEnumerate(a).ToList());

	public static double Stringify(PointerNode a) => StringAllocate(a, Enumerate(a).ToList());

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

	public static IEnumerable<double> StringEnumerate(PointerNode pointer)
	{
		Solver solver = pointer.Solver;
		double address = pointer.Address;

		for (int i = 0; true; i++)
		{
			double x = solver.GetVariable((address + i).ToString());
			if (x == 0) yield break;
			else yield return x;
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

	public static string SolveDocumentation => "Given a list represented by either its first argument as a pointer, or by all of its arguments evaluated as doubles, ";
}
