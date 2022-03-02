namespace WingCalculatorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using WingCalculatorShared.Exceptions;
using WingCalculatorShared.Nodes;

internal static class Functions
{
	private static Random _random = new();

	private static readonly Dictionary<string, StandardFunction> _functions = new List<StandardFunction>()
	{
		#region Exponential
		new("pow", (args, scope) => Math.Pow(args[0].Solve(scope), args[1].Solve(scope)), "Returns its first argument raised to the power of its second argument."),
		new("exp", (args, scope) => Math.Exp(args[0].Solve(scope)), "Returns e raised to the power of its first argument."),
		new("sqrt", (args, scope) => Math.Sqrt(args[0].Solve(scope)), "Returns the square root of its first argument."),
		new("cbrt", (args, scope) => Math.Cbrt(args[0].Solve(scope)), "Returns the cube root of its first argument."),
		new("powmod", (args, scope) => (double)BigInteger.ModPow((BigInteger)args[0].Solve(scope), (BigInteger)args[1].Solve(scope), (BigInteger)args[2].Solve(scope)), "Casts its first three arguments into BigIntegers, raises its first argument to the power of its second argument, then returns the modulus division of the result and its third argument."),

		new("log", (args, scope) =>
		{
			if (args.Count == 1) return Math.Log(args[0].Solve(scope), 10);
			else return Math.Log(args[0].Solve(scope), args[1].Solve(scope));
		}, "If there is only one argument, log returns the base 10 logarithm of its first argument. Otherwise, log returns the logarithm of its first argument in the base of its second argument."),
		new("ln", (args, scope) => Math.Log(args[0].Solve(scope)), "Returns the natural (base e) logarithm of its first argument."),
		#endregion

		#region Rounding
		new("ceil", (args, scope) => Math.Ceiling(args[0].Solve(scope)), "Returns its first argument rounded up."),
		new("floor", (args, scope) => Math.Floor(args[0].Solve(scope)), "Returns its first argument rounded down."),
		new("round", (args, scope) =>
		{
			if (args.Count == 1) return Math.Round(args[0].Solve(scope));

			double x = args[0].Solve(scope);
			double e = args[1].Solve(scope);

			double m = Math.Pow(10, e);

			return Math.Round(x * m) / m;
		}, "If there is one argument, $name rounds to the nearest integer and returns it. If there are two arguments, given a second argument e, $name rounds its first argument to the nearest 10**(-e)."),
		new("trunc", (args, scope) => Math.Truncate(args[0].Solve(scope)), "Returns its first argument rounded towards zero."),
		#endregion

		#region Trigonometry
		new("sin", (args, scope) => Math.Sin(args[0].Solve(scope)), "Returns the radian sine of its first argument."),
		new("cos", (args, scope) => Math.Cos(args[0].Solve(scope)), "Returns the radian cosine of its first argument."),
		new("tan", (args, scope) => Math.Tan(args[0].Solve(scope)), "Returns the radian tangent of its first argument."),
		new("asin", (args, scope) => Math.Asin(args[0].Solve(scope)), "Returns the inverse radian sine of its first argument."),
		new("acos", (args, scope) => Math.Acos(args[0].Solve(scope)), "Returns the inverse radian cosine of its first argument."),
		new("atan", (args, scope) => args.Count > 1 ? Math.Atan2(args[0].Solve(scope), args[1].Solve(scope)) : Math.Atan(args[0].Solve(scope)), "If there is only one argument, $name returns the inverse radian tangent of its first argument. Otherwise, $name returns the inverse radian tangent of the quotient of its first two arguments."),
		new("sinh", (args, scope) => Math.Sinh(args[0].Solve(scope)), "Returns the hyperbolic radian sine radian  of its first argument."),
		new("cosh", (args, scope) => Math.Cosh(args[0].Solve(scope)), "Returns the hyperbolic radian cosine of its first argument."),
		new("tanh", (args, scope) => Math.Tanh(args[0].Solve(scope)), "Returns the hyperbolic radian tangent of its first argument."),
		new("asinh", (args, scope) => Math.Asinh(args[0].Solve(scope)), "Returns the inverse hyperbolic radian sine of its first argument."),
		new("acosh", (args, scope) => Math.Acosh(args[0].Solve(scope)), "Returns the inverse hyperbolic radian cosine of its first argument."),
		new("atanh", (args, scope) => Math.Atanh(args[0].Solve(scope)), "Returns the inverse hyperbolic radian tangent of its first argument."),

		new("rad", (args, scope) => args[0].Solve(scope) * Math.PI / 180, "Returns its first argument converted into radians."),
		new("deg", (args, scope) => args[0].Solve(scope) * 180 / Math.PI, "Returns its first argument converted into degrees."),
		#endregion

		#region MathematicalLoops
		new("msum", (args, scope) =>
		{
			args[0].Solve(scope);
			double sum = 0;

			double end = args[1].Solve(scope);

			IAssignable assignable;
			INode counterNode;

			try
			{
				assignable = ((AssignmentNode)args[0]).A;
				counterNode = (INode)assignable;
			}
			catch
			{
				throw new WingCalcException($"The first argument of the \"msum\" function must be an assignment.", scope);
			}

			while (counterNode.Solve(scope) <= end)
			{
				sum += args[2].Solve(scope);
				assignable.Assign(counterNode.Solve(scope) + 1, scope);
			}

			return sum;
		}, "Returns the mathematical sum of its arguments, where its first argument is an assignment of the bound variable to the lower bound of summation, its second argument is the upper bound of summation, and its third argument is the expression to be summed."),
		new("mproduct", (args, scope) =>
		{
			args[0].Solve(scope);
			double product = 1;

			double end = args[1].Solve(scope);

			IAssignable assignable;
			INode counterNode;

			try
			{
				assignable = ((AssignmentNode)args[0]).A;
				counterNode = (INode)assignable;
			}
			catch
			{
				throw new WingCalcException($"The first argument of the \"mproduct\" function must be an assignment.", scope);
			}

			while (counterNode.Solve(scope) <= end)
			{
				product *= args[2].Solve(scope);
				assignable.Assign(counterNode.Solve(scope) + 1, scope);
			}

			return product;
		}, "Returns the mathematical product of its arguments, where its first argument is an assignment of the bound variable to the lower bound of multiplication, its second argument is the upper bound of multiplication, and its third argument is the expression to be multiplied."),
		#endregion

		#region Polynomial
		new("quadratic", (args, scope) =>
		{
			List<double> list;

			if (args[0] is IPointer pointer)
			{
				list = Quadratic(args[1].Solve(scope), args[2].Solve(scope), args[3].Solve(scope));
				ListHandler.Allocate(pointer, list, scope);
			}
			else
			{
				double a = args[0].Solve(scope), b = args[1].Solve(scope), c = args[2].Solve(scope);

				list = Quadratic(a, b, c);

				string positive, negative;

				if (double.IsNaN(list[0])) positive = $"({-b} + sqrt({Math.Pow(b, 2) - 4 * a * c}) / {2 * a}";
				else positive = list[0].ToString();

				if (double.IsNaN(list[1])) negative = $"({-b} - sqrt({Math.Pow(b, 2) - 4 * a * c}) / {2 * a}";
				else negative = list[1].ToString();

				scope.Solver.WriteLine($"({positive}, {negative})");
			}

			return list.Any(x => double.IsNaN(x)) ? 0 : 1;

			static List<double> Quadratic(double a, double b, double c)
			{
				double sqrt = Math.Sqrt(Math.Pow(b, 2) - 4 * a * c);

				List<double> list = new();
				list.Add((-b + sqrt) / (2 * a));
				list.Add((-b - sqrt) / (2 * a));

				return list;
			}
		}, "Finds the roots of an equation ax**2 + bx + c. If its first argument is a pointer, a = its second argument, b = its third argument, and c = its fourth argument, and the result is stored in a new list allocated at the pointer. Otherwise, a = its first argument, b = its second argument, and c = its third argument, and the result is printed to standard output. Finally, $name returns 1 if both computed values are real numbers, and 0 otherwise."),
		#endregion

		#region Comparison
		new("equals", (args, scope) =>
		{
			double error = args.Count >= 3 ? args[2].Solve(scope) : 0;

			return Math.Abs(args[0].Solve(scope) - args[1].Solve(scope)) <= error ? 1 : 0;
		}, "Returns 1 if its first and second argument are equal and otherwise returns 0. Optionally, a tolerance value within which numbers are considered equal may be provided."),
		new("nan", (args, scope) => double.IsNaN(args[0].Solve(scope)) ? 1 : 0, "Returns 1 if its first is NaN, otherwise returns 0.\nNote: This function cannot be emulated by $x == $NAN, because according to IEEE 754 floating-point specifications, NaN != NaN."),
		#endregion

		#region Probability
		new("perm", (args, scope) =>
		{
			int x = (int)args[0].Solve(scope);
			int y = (int)args[1].Solve(scope);

			return FactorialDivision(x, x - y, scope);
		}, "Performs mathematical permutation. Considering a set with a length equal to its first argument (x), $name returns the number of unique ordered subsets of length equal to its second argument (y) that can be created from that set. i.e.: xPy"),
		new("comb", (args, scope) =>
		{
			int x = (int)args[0].Solve(scope);
			int y = (int)args[1].Solve(scope);

			return FactorialDivision(x, x - y, scope) / Factorial(y, scope);
		}, "Performs mathematical combination. Considering a set with a length equal to its first argument (x), $name returns the number of unique subsets of length equal to its second argument (y) that can be created from that set, regardless of order. i.e.: xCy"),
		new("factorial", (args, scope) => Factorial((int)args[0].Solve(scope), scope), "Returns the factorial of its first argument."),
		#endregion

		#region Numeric
		new("abs", (args, scope) => Math.Abs(args[0].Solve(scope)), "Returns the absolute value of its first argument."),
		new("clamp", (args, scope) => Math.Clamp(args[0].Solve(scope), args[1].Solve(scope), args[2].Solve(scope)), "Returns its first argument if it is between its second and third arguments. Otherwise, if its first argument is less than its second, clamp returns its second argument. Finally, if its first argument is greater than its third, clamp returns its second argument."),
		new("sign", (args, scope) => Math.Sign(args[0].Solve(scope)), "Given its first argument, returns +1 if it is positive, 0 if it is 0, or -1 if it is negative."),
		new("cpsign", (args, scope) => Math.CopySign(args[0].Solve(scope), args[1].Solve(scope)), "Returns a value with the magnitude of its first argument and the sign of its second argument."),
		#endregion

		#region Bits
		new("bitinc", (args, scope) => Math.BitIncrement(args[0].Solve(scope)), "Returns its first argument incremented by the smallest computationally-possible amount."),
		new("bitdec", (args, scope) => Math.BitDecrement(args[0].Solve(scope)), "Returns its first argument decremented by the smallest computationally-possible amount."),
		#endregion

		#region ListProperties
		new("max", (args, scope) => ListHandler.Solve(args, x => x.Max(), scope), ListHandler.SolveDocumentation + "$name returns the maximum value of the list."),
		new("min", (args, scope) => ListHandler.Solve(args, x => x.Min(), scope), ListHandler.SolveDocumentation + "$name returns the minimum value of the list."),
		new("sum", (args, scope) => ListHandler.Solve(args, x => x.Sum(), scope), ListHandler.SolveDocumentation + "$name returns the sum of the values of the list."),
		new("product", (args, scope) => ListHandler.Solve(args, x => x.Product(), scope), ListHandler.SolveDocumentation + "$name returns the product of the values of the list."),
		new("mean", (args, scope) => ListHandler.Solve(args, x => x.Average(), scope), ListHandler.SolveDocumentation + "$name returns the mathematical mean of the list."),
		new("average", (args, scope) => ListHandler.Solve(args, x => x.Average(), scope), ListHandler.SolveDocumentation + "$name returns the mathematical mean of the list."),
		new("median", (args, scope) => ListHandler.Solve(args, x => x.Median(), scope), ListHandler.SolveDocumentation + "$name returns the mathematical median of the list after sorting it."),
		new("geomean", (args, scope) => ListHandler.Solve(args, x => x.GeometricMean(), scope), ListHandler.SolveDocumentation + "$name returns the geometric mean of the list."),
		new("mode", (args, scope) =>
		{
			IEnumerable<double> modes;
			if (args[0] is IPointer pointer)
			{
				if (args.Count == 1)
				{
					modes = ListHandler.Enumerate(pointer, scope).Mode();
					scope.Solver.WriteLine(modes.GetString());
				}
				else if (args[1] is IPointer second)
				{
					modes = ListHandler.Enumerate(second, scope).Mode();
					ListHandler.Allocate(pointer, modes.ToList(), scope);
				}
				else
				{
					modes = args.GetRange(1, args.Count - 1).Select(x => x.Solve(scope)).Mode();
					ListHandler.Allocate(pointer, modes.ToList(), scope);
				}
			}
			else
			{
				modes = args.Select(x => x.Solve(scope)).Mode();
				scope.Solver.WriteLine(modes.GetString());
			}

			return modes.Count();
		}, "If there is only one argument and it is a pointer, $name prints to standard output the modes of the list at the pointer represented by the first argument. If the first and second arguments are both pointers, $name computes finds the modes of the list at the pointer represented by its second argument and stores them in a list at the pointer represented by its first argument. If the first argument is a pointer and the second is not, $name finds the modes of the list represented by every argument other than the first and stores them in a list at the pointer represented by the first argument. Otherwise, $name finds the modes of all of the arguments and prints them to standard output. $name returns the number of modes found."),
		#endregion

		#region ListMemory
		new("len", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"len\" requires a pointer node as its first argument.", scope);

			return ListHandler.Length(pointer, scope);
		}, "Returns the length of the list at the pointer described by its first argument."),
		new("get", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"get\" requires a pointer node as its first argument.", scope);

			return ListHandler.Get(pointer, args[1].Solve(scope), scope);
		}, "From the list at the pointer represented by its first value, $name returns the element at the 0-based index represented by its second argument. Negative indexes are interpreted as taking from the end of the list, with the index -1 referring the last element of the list."),
		new("set", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"set\" requires a pointer node as its first argument.", scope);

			return ListHandler.Set(pointer, args[1].Solve(scope), args[2].Solve(scope), scope);
		}, "From the list at the pointer represented by its first value, $name sets the element at the 0-based index represented by its second argument to its third argument and returns its third argument. Negative indexes are interpreted as taking from the end of the list, with the index -1 referring the last element of the list."),
		new("add", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"add\" requires a pointer node as its first argument.", scope);

			return ListHandler.Add(pointer, args[1].Solve(scope), scope);
		}, "Adds and returns the value represented by its second argument to the end of the list at the pointer represented by its first argument."),
		new("insert", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"insert\" requires a pointer node as its first argument.", scope);

			List<double> made = ListHandler.Enumerate(pointer, scope).ToList(); // Yes, this is particularly inefficient. If you have a problem with it, ope n a github issue about it and I'll fix it.

			int index = (int)args[1].Solve(scope);
			if (index < 0) index += made.Count;

			double val = args[2].Solve(scope);
			try
			{
				made.Insert(index, val);
			}
			catch
			{
				throw new WingCalcException($"List at address {pointer.Address(scope)} is not big enough to insert at {index}.", scope);
			}

			ListHandler.Allocate(pointer, made, scope);

			return val;
		}, "From the list at the pointer represented by its first value, $name inserts its third argument into the 0-based index represented by its second argument and returns the inserted value. Negative indexes are interpreted as taking from the end of the list, with the index -1 referring the last element of the list."),
		new("replace", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"insert\" requires a pointer node as its first argument.", scope);

			List<double> from, to;

			if (args[1] is IPointer p1) from = ListHandler.Enumerate(p1, scope).ToList();
			else if (args[1] is QuoteNode q1) from = q1.Text.Select(x => (double)x).ToList();
			else throw new WingCalcException("Function \"replace\" requires a pointer node or a quote node as its second argument.", scope);

			if (args[2] is IPointer p2) to = ListHandler.Enumerate(p2, scope).ToList();
			else if (args[2] is QuoteNode q2) to = q2.Text.Select(x => (double)x).ToList();
			else throw new WingCalcException("Function \"replace\" requires a pointer node or a quote node as its third argument.", scope);

			List<double> list = ListHandler.Enumerate(pointer, scope).ToList();

			for (int i = 0; i < list.Count - from.Count + 1; i++)
			{
				for (int j = 0; j < from.Count && i + j < list.Count; j++)
				{
					if (list[i + j] == from[j])
					{
						if (j == from.Count - 1)
						{
							list.RemoveRange(i, from.Count);
							list.InsertRange(i, to);
							i += to.Count;
							break;
						}
					}
					else break;
				}
			}

			return ListHandler.Allocate(pointer, list, scope);
		}, "From the list at the pointer represented by the first argument, $name replaces each sequence found that matches the list generated from its second argument (which is a quote or pointer) and replaces it with the list generated from its third argument (which is also a quote or pointer)."),
		new("indexof", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"indexof\" requires a pointer node as its first argument.", scope);

			return ListHandler.IndexOf(pointer, args[1].Solve(scope), scope);
		}, "Returns the 0-based index of its second argument as found within the list at the pointer represented by its first argument, or -1 if that list does not contain the second argument of indexof."),
		new("remove", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"remove\" requires a pointer node as its first argument.", scope);

			return ListHandler.Remove(pointer, args[1].Solve(scope), scope);
		}, "If the value represented by its second argument can be found within the list at the pointer represented by its first argument, $name removes the first occurence of that value and returns 1. Otherwise, remove returns 0."),
		new("clear", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"clear\" requires a pointer node as its first argument.", scope);

			return ListHandler.Clear(pointer, scope);
		}, "Clears the list at the pointer represented by its first argument and returns the address of the pointer."),
		new("contains", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"contains\" requires a pointer node as its first argument.", scope);

			return ListHandler.Enumerate(pointer, scope).Contains(args[1].Solve(scope)) ? 1 : 0;
		}, "Returns 1 if its second argument is contained within the list at the pointer represented by the first argument, otherwise returns 0."),
		new("concat", (args, scope) =>
		{
			IPointer a = args[0] as IPointer ?? throw new WingCalcException("Function \"concat\" requires a pointer node as its first argument.", scope);
			IPointer b = args[1] as IPointer ?? throw new WingCalcException("Function \"concat\" requires a pointer node as its second argument.", scope);
			IPointer c = args.Count >= 3
				? args[2] as IPointer ?? throw new WingCalcException("Function \"concat\" requires a pointer node as its third argument.", scope)
				: a;

			return ListHandler.Concat(a, b, c, scope);
		}, "If there are two arguments, $name adds each element of the list at the pointer represented by the second argument to the list at the pointer represented by the first argument. Otherwise, $name creates a new list at the pointer represented by the third argument and adds each element of the list at the pointer represented by the first argument to it, then adds each element of the list at the pointer represented by the second argument to it. Finally, $name returns the address of the pointer containing the concatenated list."),
		new("copy", (args, scope) =>
		{
			IPointer a = args[0] as IPointer ?? throw new WingCalcException("Function \"copy\" requires a pointer node as its first argument.", scope);
			IPointer b = args[1] as IPointer ?? throw new WingCalcException("Function \"copy\" requires a pointer node as its second argument.", scope);

			return ListHandler.Copy(a, b, scope);
		}, "Creates a copy of the list at the pointer represented by the first argument to the pointer represented by the second argument and returns the address of the pointer the list was copied to."),
		new("setify", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"setify\" requires a pointer node as its first argument.", scope);

			return ListHandler.Setify(pointer, scope);
		}, "Removes all duplicate elements from the list at the pointer represented by the first argument and returns the address of the pointer."),
		new("sort", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"sort\" requires a pointer node as its first argument.", scope);

			return ListHandler.Allocate(pointer, ListHandler.Enumerate(pointer, scope).OrderBy(x => x).ToList(), scope);
		}, "Sorts the list at the pointer represented by the first argument and returns the address of the pointer."),
		new("iter", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"iter\" requires a pointer node as its first argument.", scope);
			IAssignable lambdaVar = args[1] as IAssignable ?? throw new WingCalcException("Function \"iter\" requires an assignable node as its second argument.", scope);

			if (args.Count >= 4)
			{
				IAssignable indexVar = args[2] as IAssignable ?? throw new WingCalcException("Function \"iter\" requires an assignable node as its third argument.", scope);
				foreach (var (x, i) in ListHandler.Enumerate(pointer, scope).Select((x, i) => (x, i)))
				{
					lambdaVar.Assign(x, scope);
					indexVar.Assign(i, scope);
					args[3].Solve(scope);
				}
			}
			else
			{
				foreach (double x in ListHandler.Enumerate(pointer, scope))
				{
					lambdaVar.Assign(x, scope);
					args[2].Solve(scope);
				}
			}

			return ListHandler.Enumerate(pointer, scope).Count();
		}, "Enumerates through the list at the pointer represented by its first argument, assigning each value found to the assignable node represented by its second argument. If there are three arguments, $name evaluates the expression represented by its third argument for each element of the list. Otherwise, $name assigns the index of each value found to the assignable node represented by the second argument, and evaluates the expression represented by its fourth argument for each element of the list. Finally, $name returns the number of elements contained within the list."),
		new("filter", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"filter\" requires a pointer node as its first argument.", scope);
			IAssignable lambdaVar = args[1] as IAssignable ?? throw new WingCalcException("Function \"filter\" requires an assignable node as its second argument.", scope);

			List<double> filtered = new();

			if (args.Count >= 4)
			{
				IAssignable indexVar = args[2] as IAssignable ?? throw new WingCalcException("Function \"filter\" requires an assignable node as its third argument.", scope);
				foreach (var (x, i) in ListHandler.Enumerate(pointer, scope).Select((x, i) => (x, i)))
				{
					lambdaVar.Assign(x, scope);
					indexVar.Assign(i, scope);

					if (args[3].Solve(scope) != 0)
					{
						filtered.Add(x);
					}
				}
			}
			else
			{
				foreach (double x in ListHandler.Enumerate(pointer, scope))
				{
					lambdaVar.Assign(x, scope);

					if (args[2].Solve(scope) != 0)
					{
						filtered.Add(x);
					}
				}
			}

			ListHandler.Allocate(pointer, filtered, scope);

			return ListHandler.Enumerate(pointer, scope).Count();
		}, "Enumerates through the list at the pointer represented by its first argument, assigning each value found to the assignable node represented by its second argument. If there are three arguments, $name evaluates the expression represented by its third argument for each element of the list. Otherwise, $name assigns the index of each value found to the assignable node represented by the second argument, and evaluates the expression represented by its fourth argument for each element of the list. For each element in the list, if the expression evaluated to 0, that element is removed from the list. Finally, $name returns address of the pointer represented by its first argument."),
		new("any", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"any\" requires a pointer node as its first argument.", scope);
			IAssignable lambdaVar = args[1] as IAssignable ?? throw new WingCalcException("Function \"any\" requires an assignable node as its second argument.", scope);

			if (args.Count >= 4)
			{
				IAssignable indexVar = args[2] as IAssignable ?? throw new WingCalcException("Function \"any\" requires an assignable node as its third argument.", scope);
				foreach (var (x, i) in ListHandler.Enumerate(pointer, scope).Select((x, i) => (x, i)))
				{
					lambdaVar.Assign(x, scope);
					indexVar.Assign(i, scope);

					if (args[3].Solve(scope) != 0)
					{
						return 1;
					}
				}
			}
			else
			{
				foreach (double x in ListHandler.Enumerate(pointer, scope))
				{
					lambdaVar.Assign(x, scope);

					if (args[2].Solve(scope) != 0)
					{
						return 1;
					}
				}
			}

			return 0;
		}, "Enumerates through the list at the pointer represented by its first argument until the given expression doesn't evaluate to 0, or until the end of the list is found, assigning each value found to the assignable node represented by its second argument. If there are three arguments, $name evaluates the expression represented by its third argument for each element of the list. Otherwise, $name assigns the index of each value found to the assignable node represented by the second argument, and evaluates the expression represented by its fourth argument for each element of the list. Finally, $name returns 1 if an element matching the expression was found, and 0 otherwise."),
		new("count", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"count\" requires a pointer node as its first argument.", scope);
			IAssignable lambdaVar = args[1] as IAssignable ?? throw new WingCalcException("Function \"count\" requires an assignable node as its second argument.", scope);

			int count = 0;

			if (args.Count >= 4)
			{
				IAssignable indexVar = args[2] as IAssignable ?? throw new WingCalcException("Function \"count\" requires an assignable node as its third argument.", scope);
				foreach (var (x, i) in ListHandler.Enumerate(pointer, scope).Select((x, i) => (x, i)))
				{
					lambdaVar.Assign(x, scope);
					indexVar.Assign(i, scope);

					if (args[3].Solve(scope) != 0)
					{
						count++;
					}
				}
			}
			else
			{
				foreach (double x in ListHandler.Enumerate(pointer, scope))
				{
					lambdaVar.Assign(x, scope);

					if (args[2].Solve(scope) != 0)
					{
						count++;
					}
				}
			}

			return count;
		}, "Enumerates through the list at the pointer represented by its first argument, assigning each value found to the assignable node represented by its second argument. If there are three arguments, $name evaluates the expression represented by its third argument for each element of the list. Otherwise, $name assigns the index of each value found to the assignable node represented by the second argument, and evaluates the expression represented by its fourth argument for each element of the list. For each element in the list, if the expression evaluated to 0, that element is removed from the list. Finally, $name returns the number of elements in the list for which the expression did not evaluate to 0."),
		#endregion

		#region ControlFlow
		new("if", (args, scope) =>
		{
			if (args.Count == 2) return args[0].Solve(scope) != 0 ? args[1].Solve(scope) : 0;
			else return args[0].Solve(scope) != 0 ? args[1].Solve(scope) : args[2].Solve(scope);
		}, "If its first argument does not evaluate to 0, $name returns its second argument. If its first argument evaluates to 0, if there are two arguments $name returns 0, or otherwise evaluates and returns its third argument."),
		new("else", (args, scope) => args[0].Solve(scope) == 0 ? args[1].Solve(scope) : 1, "If its first argument evaluates to 0, $name returns its second argument. Otherwise, it returns 1."),
		new("switch", (args, scope) =>
		{
			double x = args[0].Solve(scope);

			for (int i = 1; i < args.Count - 1; i += 2)
			{
				if (x == args[i].Solve(scope))
				{
					return args[i + 1].Solve(scope);
				}
			}

			if (args.Count % 2 == 0)
			{
				return args[^1].Solve(scope);
			}
			else return 0;
		}, "Evaluates its first argument and for each of the following pairs of arguments, $name compares it to the first argument and returns the second if the two are equal. If the value is not equal to the first of any of the pairs, the return value depends on the number of arguments. If there are an even number of arguments, $name returns the last argument. Otherwise, it returns zero."),
		new("for", (args, scope) =>
		{
			args[0].Solve(scope);
			int count = 0;

			while (args[1].Solve(scope) != 0)
			{
				args[3].Solve(scope);
				args[2].Solve(scope);
				count++;
			}

			return count;
		}, "Evaluates its first argument. Then, while its second argument doesn't evaluate to 0, $name evaluates its fourth argument and then its third argument. Finally, $name returns the number of iterations that occurred."),
		new("while", (args, scope) =>
		{
			int count = 0;

			while (args[0].Solve(scope) != 0)
			{
				args[1].Solve(scope);
				count++;
			}

			return count;
		}, "While its first argument doesn't evaluate to 0, $name evaluates its second argument. Finally, $name returns the number of iterations that occurred."),
		new("dowhile", (args, scope) =>
		{
			int count = 0;

			do
			{
				args[1].Solve(scope);
				count++;
			}
			while (args[0].Solve(scope) != 0);

			return count;
		}, "Evaluates its second argument. Then, while its first argument doesn't evaluate to 0, $name evaluates its second argument. Finally, $name returns the number of iterations that occurred."),
		new("repeat", (args, scope) =>
		{
			double count = args[1].Solve(scope);

			for (int i = 0; i < count; i++)
			{
				args[0].Solve(scope);
			}

			return count;
		}, "$name evaluates its second argument, then repeatedly evaluates its first argument that many times. Finally, $name returns the number of iterations that occurred."),
		#endregion

		#region Memory
		new("alloc", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"alloc\" requires a pointer node as its first argument.", scope);

			List<double> values;

			if (args[1] is QuoteNode quote)
			{
				values = quote.Text.Select(c => (double)c).ToList();
			}
			else
			{
				values = args.Skip(1).Select(x => x.Solve(scope)).ToList();
			}

			return ListHandler.Allocate(pointer, values, scope);
		}, "If its second argument is a quote, $name allocates each character of that quote to a new list at the pointer represented by its first argument. Otherwise, $name allocates all of its arguments (other than the first) to a new list at the pointer represented by its first argument."),
		new("calloc", (args, scope) => ListHandler.Allocate(args[0] as IPointer ?? throw new WingCalcException("Function \"calloc\" requires a pointer node as its first argument.", scope), new double[10].ToList(), scope), "Interprets its first argument as a pointer and allocates to it a list of zeroes with a length equal to its second argument."),
		new("malloc", (args, scope) => scope.Solver.SetVariable((args[0] as IPointer ?? throw new WingCalcException("Function \"malloc\" requires a pointer node as its first argument.", scope)).Address(scope).ToString(), args[1].Solve(scope)), "Interprets its first argument as a pointer and allocates to it a list of uninitialized values with a length equal to its second argument."),
		new("range", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"range\" requires a pointer node as its first argument.", scope);

			double start, end;

			if (args.Count >= 3)
			{
				start = args[1].Solve(scope);
				end = args[2].Solve(scope);
			}
			else
			{
				start = 0;
				end = args[1].Solve(scope);
			}

			List<double> vals = new();
			for (double i = start; i < end; i++)
			{
				vals.Add(i);
			}

			return ListHandler.Allocate(pointer, vals, scope);
		}, "If there are two parameters, $range allocates each integer in the range [0, second argument) to a list at the pointer represented by its first argument. Otherwise, $range allocates each integer in the range [second argument, third argument) to a list at the pointer represented by its first argument."),
		new("memprint", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"memprint\" requires a pointer node as its first argument.", scope);

			Solver solver = scope.Solver;
			double address = pointer.Address(scope);
			double value = solver.GetVariable(address.ToString());

			solver.WriteLine($"${address} = {value}");
			return value;
		}, "Given its first argument as a pointer, $name prints the address of the pointer and the value at the pointer to standard output. Finally, $name returns the value at the pointer."),
		new("print", (args, scope) =>
		{
			IPointer pointer = args[0] as IPointer ?? throw new WingCalcException("Function \"print\" requires a pointer node as its first argument.", scope);

			scope.Solver.WriteLine(ListHandler.Enumerate(pointer, scope).GetString());

			return ListHandler.Length(pointer, scope);
		}, "Given its first argument as a pointer, $name enumerates the list found at the pointer and prints each element within it to standard out. Finally, $name returns the number of values printed."),
		#endregion

		#region Strings
		new("exec", (args, scope) =>
		{
			if (args[0] is IPointer pointer)
			{
				StringBuilder sb = new();
				foreach (double x in ListHandler.Enumerate(pointer, scope)) sb.Append((char)x);
				return scope.Solver.Solve(sb.ToString());
			}
			else if (args[0] is QuoteNode quote)
			{
				return scope.Solver.Solve(quote.Text);
			}
			else
			{
				throw new WingCalcException("Function \"exec\" requires a pointer node or a quote node as its first argument.", scope);
			}
		}, "Given its first argument as a pointer, $name reads the list at that pointer as a string and executes it as a WingCalc expression. Or, given its first argument as a quote, $name executes the text of the quote as a WingCalc expression."),
		#endregion

		#region Factors
		new("gcd", (args, scope) => ListHandler.Solve(args, x => (double)x.Aggregate((a, b) => (double)Factorizer.GCD((BigInteger)a, (BigInteger)b)), scope), ListHandler.SolveDocumentation + "$name computes and returns the greatest common denominator of the list."),
		new("lcm", (args, scope) => ListHandler.Solve(args, x => (double)x.Aggregate((a, b) => (double)Factorizer.LCM((BigInteger)a, (BigInteger)b)), scope), ListHandler.SolveDocumentation + "$name computes and returns the least common multiple of the list."),
		new("factor", (args, scope) =>
		{
			List<double> factors;

			if (args[0] is IPointer pointer)
			{
				factors = Factorizer.Factors((BigInteger)args[1].Solve(scope)).Select(x => (double)x).ToList();

				ListHandler.Allocate(pointer, factors, scope);
			}
			else
			{
				factors = Factorizer.Factors((BigInteger)args[0].Solve(scope)).Select(x => (double)x).ToList();

				scope.Solver.WriteLine($"{{ {string.Join(", ", factors)} }}");
			}

			return factors.Count;
		}, "If there is only one argument, $name prints to standard output the factors of its first argument. Otherwise, $name creates a list at the pointer represented by its first argument and fills it with the factors of its second argument."),
		new("prime", (args, scope) => Factorizer.IsPrime((BigInteger)args[0].Solve(scope)) ? 1 : 0, "Returns 1 if its first argument is prime, and 0 otherwise."),
		new("primefactor", (args, scope) =>
		{
			List<double> factors;

			if (args[0] is IPointer pointer)
			{
				factors = Factorizer.PrimeFactors((BigInteger)args[1].Solve(scope)).Select(x => (double)x).ToList();

				ListHandler.Allocate(pointer, factors, scope);
			}
			else
			{
				factors = Factorizer.PrimeFactors((BigInteger)args[0].Solve(scope)).Select(x => (double)x).ToList();

				scope.Solver.WriteLine($"{{ {string.Join(", ", factors)} }}");
			}

			return factors.Count;
		}, "If there is only one argument, $name prints to standard output the prime factorization of its first argument. Otherwise, $name creates a list at the pointer represented by its first argument and fills it with the prime factorization of its second argument."),
		#endregion

		#region Programming
		new("eval", (args, scope) => args[0].Solve(scope), "Evaluates and returns its first argument."),
		new("call", (args, scope) =>
		{
			ICallable callable = args[0] as ICallable ?? throw new WingCalcException($"The first argument of the \"call\" function must be callable.", scope);

			if (args.Count == 1) return callable.Call(scope, new());
			else return callable.Call(scope, new(args.GetRange(1, args.Count - 1)));
		}, ""),
		new("val", (args, scope) =>
		{
			IAssignable assignable = args[0] as IAssignable ?? throw new WingCalcException($"The first argument of the \"val\" function must be assignable.", scope);

			return assignable.Assign(args[1].Solve(scope), scope);
		}, "Evaluates its second argument and assigns it to its first argument."),
		new("ignore", (args, scope) => 0, "Does nothing."),
		new("deepset", (args, scope) =>
		{
			IAssignable assignable = args[0] as IAssignable ?? throw new WingCalcException($"The first argument of the \"deepset\" function must be assignable.", scope);

			return assignable.DeepAssign(args[1], scope);
		}, "Recursively finds the deepest assignable node within its first argument and sets it to a reference to its second argument."),
		new("deepval", (args, scope) =>
		{
			IAssignable assignable = args[0] as IAssignable ?? throw new WingCalcException($"The first argument of the \"deepset\" function must be assignable.", scope);

			return assignable.DeepAssign(args[1].Solve(scope), scope);
		}, "Recursively finds the deepest assignable node within its first argument and sets it to its second argument evaluated."),
		#endregion

		#region ExceptionHandling
		new("catch", (args, scope) =>
		{
			try
			{
				return args[0].Solve(scope);
			}
			catch
			{
				return args[1].Solve(scope);
			}
		}, "Attempts to evaluate and return its first argument. If any exception is thrown, evaluates and returns its second argument instead."),
		new("catchwc", (args, scope) =>
		{
			try
			{
				return args[0].Solve(scope);
			}
			catch (WingCalcException)
			{
				return args[1].Solve(scope);
			}
		}, "Attempts to evaluate and return its first argument. If a WingCalcException is thrown, evaluates and returns its second argument instead."),
		new("catchcs", (args, scope) =>
		{
			try
			{
				return args[0].Solve(scope);
			}
			catch (Exception ex) when (ex is not WingCalcException and not CustomException)
			{
				return args[1].Solve(scope);
			}
		}, "Attempts to evaluate and return its first argument. If any C# exception is thrown, evaluates and returns its second argument instead."),
		new("throw", (args, scope) =>
		{
			QuoteNode quote = args[0] as QuoteNode ?? throw new WingCalcException($"The first argument of \"throw\" must be a QuoteNode.", scope);

			throw new CustomException(quote.Text);
		}, "Given a quote as its first argument, throws a new CustomException with that quote's text as its message."),
		#endregion

		#region Random
		new("rand", (args, scope) =>
		{
			if (args.Count == 1) return _random.Next((int)args[0].Solve(scope));
			else return _random.Next((int)args[0].Solve(scope), (int)args[1].Solve(scope));
		}, "If there is one argument, $name generates and returns an integer less than its first argument rounded towards zero. Otherwise, generates and returns a number greater than or equal to its first argument rounded towards zero, but less than its second argument rounded towards zero."),
		new("drand", (args, scope) => _random.NextDouble() * args[0].Solve(scope), "Returns a random number within the range [0.0, 1.0) multiplied by its first argument."),
		new("srand", (args, scope) =>
		{
			int x = (int)args[0].Solve(scope);
			_random = new(x);
			return x;
		}, "Sets the random seed to its first argument and returns the new seed."),
		#endregion

		#region Write
		new("write", (args, scope) =>
		{
			if (args[0] is QuoteNode quote)
			{
				scope.Solver.Write(quote.Text);

				return quote.Text.Length;
			}
			else if (args[0] is IPointer pointer)
			{
				string text = new(ListHandler.Enumerate(pointer, scope).Select(x => (char)x).ToArray());
				scope.Solver.Write(text);

				return text.Length;
			}
			else
			{
				throw new WingCalcException("Function \"write\" requires a pointer node or a quote node as its first argument.", scope);
			}
		}, "Given a quote as its first argument, $name prints the text of the quote to standard output. Or, given a pointer as its first argument, $name interprets the list at the pointer as text and prints it to standard output. Finally, $name returns the number of characters printed to standard output."),
		new("writeline", (args, scope) =>
		{
			if (args[0] is QuoteNode quote)
			{
				scope.Solver.WriteLine(quote.Text);

				return quote.Text.Length;
			}
			else if (args[0] is IPointer pointer)
			{
				string text = new(ListHandler.Enumerate(pointer, scope).Select(x => (char)x).ToArray());
				scope.Solver.WriteLine(text);

				return text.Length;
			}
			else
			{
				throw new WingCalcException("Function \"writeline\" requires a pointer node or a quote node as its first argument.", scope);
			}
		}, "Given a quote as its first argument, $name prints the text of the quote to standard output followed by a newline. Or, given a pointer as its first argument, $name interprets the list at the pointer as text and prints it to standard output followed by a newline. Finally, $name returns the number of characters printed to standard output (not including the added newline)."),
		#endregion

		#region Flush/Clear
		new("clearout", (args, scope) =>
		{
			scope.Solver.Clear();
			return 1;
		}, "Clears standard output and returns 1."),
		new("flush", (args, scope) =>
		{
			scope.Solver.Flush();
			return 1;
		}, "Flushes any buffered data from standard output and returns 1."),
		#endregion

		#region Meta
		new("help", (args, scope) =>
		{
			QuoteNode quote = args[0] as QuoteNode ?? throw new WingCalcException("Function \"help\" requires a quote node as its first argument.", scope);

			string text = quote.Text.Trim().ToLower();

			if (_functions.ContainsKey(text))
			{
				scope.Solver.WriteLine($"Documentation for {quote.Text}: {_functions[quote.Text].Documentation.Replace("$name", quote.Text)}");
			}
			else if (Operators.GetDocumentation(text, out string opDocs))
			{
				scope.Solver.WriteLine($"Documentation for {quote.Text}: {opDocs}");
			}
			else
			{
				throw new WingCalcException($"Function or operator \"{quote.Text}\" does not exist.", scope);
			}

			return 42;
		}, "Given a quote representing a function name or operator as its first argument, $name prints the documentation of the function or operator to standard output. Finally, $name returns 42."),
		new("stdlist", (args, scope) =>
		{
			scope.Solver.WriteLine($"{{ {string.Join(", ", _functions.Keys)} }}");

			return _functions.Count;
		}, "Prints every function name in the standard library and returns the number of functions in the standard library."),
		new("oplist", (args, scope) =>
		{
			scope.Solver.WriteLine($"{{ {Operators.ListOperators()} }}");

			return _functions.Count;
		}, "Prints every the symbols of each operator in WingCalc, then returns the number of operators in WingCalc."),
		#endregion

	}.ToDictionary(x => x.Name, x => x);

	internal static Function Get(string s) => _functions[s].Function;

	private static long FactorialDivision(int x, int y, Scope scope)
	{
		if (x <= 0) throw new WingCalcException("Cannot compute permutation/combination with non-positive n.", scope);
		if (y < 0) throw new WingCalcException("Cannot compute permutation/combination with negative n - k.", scope);
		if (y > x) throw new WingCalcException("Cannot compute permutation/combination with k > n", scope);

		long result = 1;
		for (int i = x; i > y; i--)
		{
			result *= i;
		}

		return result;
	}

	private static long Factorial(int x, Scope scope)
	{
		if (x < 0) throw new WingCalcException("Cannot calculate the factorial of a negative number.", scope);

		long result = 1;

		for (int i = x; i > 1; i--) result *= i;

		return result;
	}

	public delegate double Function(List<INode> list, Scope scope);
	record struct StandardFunction(string Name, Function Function, string Documentation);
}
