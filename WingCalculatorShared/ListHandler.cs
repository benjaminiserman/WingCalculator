namespace WingCalculatorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using WingCalculatorShared.Nodes;

internal static class ListHandler
{
	public static double Allocate(IPointer pointer, IList<double> values, Scope scope)
	{
		double address = pointer.Address(scope);

		pointer.Set(address.ToString(), values.Count, scope);

		for (int i = 1; i <= values.Count; i++)
		{
			pointer.Set((address + i).ToString(), values[i - 1], scope);
		}

		return address;
	}

	public static double StringAllocate(IPointer pointer, IList<double> values, Scope scope)
	{
		double address = pointer.Address(scope);

		for (int i = 0; i < values.Count; i++)
		{
			pointer.Set((address + i).ToString(), values[i], scope);
		}

		pointer.Set((address + values.Count).ToString(), 0, scope);

		return address;
	}

	public static double Length(IPointer pointer, Scope scope) => pointer.Get(pointer.Address(scope).ToString(), scope);

	public static double Get(IPointer pointer, double i, Scope scope)
	{
		if (i < 0) i = Length(pointer, scope) + i;
		return pointer.Get((pointer.Address(scope) + i + 1).ToString(), scope);
	}

	public static double Set(IPointer pointer, double i, double x, Scope scope)
	{
		if (i < 0) i = Length(pointer, scope) + i;
		return pointer.Set((pointer.Address(scope) + i + 1).ToString(), x, scope);
	}

	public static double Add(IPointer pointer, double x, Scope scope)
	{
		double address = pointer.Address(scope);

		double length = pointer.Get(address.ToString(), scope) + 1;
		pointer.Set(address.ToString(), length, scope);
		pointer.Set((address + length).ToString(), x, scope);

		return x;
	}

	public static double IndexOf(IPointer pointer, double x, Scope scope)
	{
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

	public static double Remove(IPointer pointer, double x, Scope scope)
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

	public static double Clear(IPointer pointer, Scope scope) => Allocate(pointer, new List<double>(), scope);

	public static double Concat(IPointer a, IPointer b, IPointer c, Scope scope) => Allocate(c, Enumerate(a, scope).Concat(Enumerate(b, scope)).ToList(), scope);

	public static double Copy(IPointer a, IPointer b, Scope scope) => Allocate(b, Enumerate(a, scope).ToList(), scope);

	public static double Setify(IPointer a, Scope scope) => Allocate(a, Enumerate(a, scope).ToHashSet().ToList(), scope);

	public static double Listify(IPointer a, Scope scope) => Allocate(a, StringEnumerate(a, scope).ToList(), scope);

	public static double Stringify(IPointer a, Scope scope) => StringAllocate(a, Enumerate(a, scope).ToList(), scope);

	public static IEnumerable<double> Enumerate(IPointer pointer, Scope scope)
	{
		double address = pointer.Address(scope);

		double length = pointer.Get(address.ToString(), scope);

		for (int i = 1; i <= length; i++)
		{
			yield return pointer.Get((address + i).ToString(), scope);
		}
	}

	public static IEnumerable<double> StringEnumerate(IPointer pointer, Scope scope)
	{
		double address = pointer.Address(scope);

		for (int i = 0; true; i++)
		{
			double x = pointer.Get((address + i).ToString(), scope);
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
		if (args[0] is IPointer pointer) return func(Enumerate(pointer, scope));
		else return func(args.Select(x => x.Solve(scope)));
	}

	public static string GetString(this IEnumerable<double> list) => $"{{ {string.Join(", ", list)} }}";

	public static string SolveDocumentation => "Given a list represented by either its first argument as a pointer, or by all of its arguments evaluated as doubles, ";
}
