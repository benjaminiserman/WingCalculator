namespace WingCalculatorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using WingCalculatorShared.Nodes;

internal static class ListHandler
{
	public static double Allocate(PointerNode pointer, IList<double> values, Scope scope)
	{
		Solver solver = scope.Solver;
		double address = pointer.Address(scope);

		solver.SetVariable(address.ToString(), values.Count);

		for (int i = 1; i <= values.Count; i++)
		{
			solver.SetVariable((address + i).ToString(), values[i - 1]);
		}

		return address;
	}

	public static double StringAllocate(PointerNode pointer, IList<double> values, Scope scope)
	{
		Solver solver = scope.Solver;
		double address = pointer.Address(scope);

		for (int i = 0; i < values.Count; i++)
		{
			solver.SetVariable((address + i).ToString(), values[i]);
		}

		solver.SetVariable((address + values.Count).ToString(), 0);

		return address;
	}

	public static double Length(PointerNode pointer, Scope scope) => scope.Solver.GetVariable(pointer.Address(scope).ToString());

	public static double Get(PointerNode pointer, double i, Scope scope)
	{
		if (i < 0) i = Length(pointer, scope) + i;
		return scope.Solver.GetVariable((pointer.Address(scope) + i + 1).ToString());
	}

	public static double Set(PointerNode pointer, double i, double x, Scope scope)
	{
		if (i < 0) i = Length(pointer, scope) + i;
		return scope.Solver.SetVariable((pointer.Address(scope) + i + 1).ToString(), x);
	}

	public static double Add(PointerNode pointer, double x, Scope scope)
	{
		Solver solver = scope.Solver;
		double address = pointer.Address(scope);

		double length = solver.GetVariable(address.ToString()) + 1;
		solver.SetVariable(address.ToString(), length);
		solver.SetVariable((address + length).ToString(), x);

		return x;
	}

	public static double IndexOf(PointerNode pointer, double x, Scope scope)
	{
		Solver solver = scope.Solver;
		double address = pointer.Address(scope);

		try
		{
			return Enumerate(pointer, scope).Select((x, i) => (x, i)).First(y => y.x == x).i;
		}
		catch
		{
			return -1;
		}
	}

	public static double Remove(PointerNode pointer, double x, Scope scope)
	{
		var list = Enumerate(pointer, scope).ToList();
		if (!list.Contains(x)) return 0;
		else
		{
			list.Remove(x);
			Allocate(pointer, list, scope);
			return 1;
		}
	}

	public static double Clear(PointerNode pointer, Scope scope) => Allocate(pointer, new List<double>(), scope);

	public static double Concat(PointerNode a, PointerNode b, PointerNode c, Scope scope) => Allocate(c, Enumerate(a, scope).Concat(Enumerate(b, scope)).ToList(), scope);

	public static double Copy(PointerNode a, PointerNode b, Scope scope) => Allocate(b, Enumerate(a, scope).ToList(), scope);

	public static double Setify(PointerNode a, Scope scope) => Allocate(a, Enumerate(a, scope).ToHashSet().ToList(), scope);

	public static double Listify(PointerNode a, Scope scope) => Allocate(a, StringEnumerate(a, scope).ToList(), scope);

	public static double Stringify(PointerNode a, Scope scope) => StringAllocate(a, Enumerate(a, scope).ToList(), scope);

	public static IEnumerable<double> Enumerate(PointerNode pointer, Scope scope)
	{
		Solver solver = scope.Solver;
		double address = pointer.Address(scope);

		double length = solver.GetVariable(address.ToString());

		for (int i = 1; i <= length; i++)
		{
			yield return solver.GetVariable((address + i).ToString());
		}
	}

	public static IEnumerable<double> StringEnumerate(PointerNode pointer, Scope scope)
	{
		Solver solver = scope.Solver;
		double address = pointer.Address(scope);

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

	public static IEnumerable<double> Mode(this IEnumerable<double> list) => new HashSet<double>(list.GroupBy(v => list.Count(x => x == v)).OrderByDescending(g => g.Key).First().Select(x => x));

	public static double Product(this IEnumerable<double> list) => list.Aggregate((a, b) => a * b);

	public static double GeometricMean(this IEnumerable<double> list) => Math.Pow(list.Product(), 1.0 / list.Count());

	public static double Solve(IList<INode> args, Func<IEnumerable<double>, double> func, Scope scope)
	{
		if (args[0] is PointerNode pointer) return func(Enumerate(pointer, scope));
		else return func(args.Select(x => x.Solve(scope)));
	}

	public static string GetString(this IEnumerable<double> list) => $"{{ {string.Join(", ", list)} }}";

	public static string SolveDocumentation => "Given a list represented by either its first argument as a pointer, or by all of its arguments evaluated as doubles, ";
}
