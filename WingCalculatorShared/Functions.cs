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
	public static Solver Solver { get; set; } // I don't love this solution
	private static Random _random = new();

	private static readonly Dictionary<string, Func<List<INode>, double>> _functions = new()
	{
		#region Exponential
		["pow"] = args => Math.Pow(args[0].Solve(), args[1].Solve()),
		["exp"] = args => Math.Exp(args[0].Solve()),
		["sqrt"] = args => Math.Sqrt(args[0].Solve()),
		["cbrt"] = args => Math.Cbrt(args[0].Solve()),
		["powmod"] = args => (double)BigInteger.ModPow((BigInteger)args[0].Solve(), (BigInteger)args[1].Solve(), (BigInteger)args[2].Solve()),

		["log"] = args =>
		{
			if (args.Count == 1) return Math.Log(args[0].Solve(), 10);
			else return Math.Log(args[0].Solve(), args[1].Solve());
		},
		["ln"] = args => Math.Log(args[0].Solve()),
		#endregion

		#region Rounding
		["ceil"] = args => Math.Ceiling(args[0].Solve()),
		["floor"] = args => Math.Floor(args[0].Solve()),
		["round"] = args => Math.Round(args[0].Solve(), args.Count > 1 ? (int)args[1].Solve() : 0, (MidpointRounding)(args.Count > 2 ? args[2].Solve() : 0)),
		["trunc"] = args => (int)args[0].Solve(),
		#endregion

		#region Trigonometry
		["sin"] = args => Math.Sin(args[0].Solve()),
		["cos"] = args => Math.Cos(args[0].Solve()),
		["tan"] = args => Math.Tan(args[0].Solve()),
		["asin"] = args => Math.Asin(args[0].Solve()),
		["acos"] = args => Math.Acos(args[0].Solve()),
		["atan"] = args => args.Count > 1 ? Math.Atan2(args[0].Solve(), args[1].Solve()) : Math.Atan(args[0].Solve()),
		["sinh"] = args => Math.Sinh(args[0].Solve()),
		["cosh"] = args => Math.Cosh(args[0].Solve()),
		["tanh"] = args => Math.Tanh(args[0].Solve()),
		["arcsinh"] = args => Math.Asinh(args[0].Solve()),
		["arccosh"] = args => Math.Acosh(args[0].Solve()),
		["arctanh"] = args => Math.Atanh(args[0].Solve()),

		["rad"] = args => args[0].Solve() * Math.PI / 180,
		["deg"] = args => args[0].Solve() * 180 / Math.PI,
		#endregion

		#region MathematicalLoops
		["msum"] = args =>
		{
			args[0].Solve();
			double sum = 0;

			double end = args[1].Solve();

			IAssignable assignable;
			INode counterNode;

			try
			{
				assignable = ((AssignmentNode)args[0]).A;
				counterNode = (INode)assignable;
			}
			catch
			{
				throw new WingCalcException($"The first argument of the \"msum\" must be an assignment.");
			}

			while (counterNode.Solve() <= end)
			{
				sum += args[2].Solve();
				assignable.Assign(counterNode.Solve() + 1);
			}

			return sum;
		},
		["mproduct"] = args =>
		{
			args[0].Solve();
			double product = 1;

			double end = args[1].Solve();

			IAssignable assignable;
			INode counterNode;

			try
			{
				assignable = ((AssignmentNode)args[0]).A;
				counterNode = (INode)assignable;
			}
			catch
			{
				throw new WingCalcException($"The first argument of the \"mproduct\" must be an assignment.");
			}

			while (counterNode.Solve() <= end)
			{
				product *= args[2].Solve();
				assignable.Assign(counterNode.Solve() + 1);
			}

			return product;
		},
		#endregion

		#region Comparison
		["equals"] = args =>
		{
			double error = args.Count >= 3 ? args[2].Solve() : 0;

			return Math.Abs(args[0].Solve() - args[1].Solve()) <= error ? 1 : 0;
		},
		["nan"] = args => double.IsNaN(args[0].Solve()) ? 1 : 0,
		#endregion

		#region Probability
		["perm"] = args =>
		{
			int x = (int)args[0].Solve();
			int y = (int)args[1].Solve();

			return FactorialDivision(x, x - y);
		},
		["comb"] = args =>
		{
			int x = (int)args[0].Solve();
			int y = (int)args[1].Solve();

			return FactorialDivision(x, x - y) / Factorial(y);
		},
		["factorial"] = args => Factorial((int)args[0].Solve()),
		#endregion

		#region Numeric
		["abs"] = args => Math.Abs(args[0].Solve()),
		["clamp"] = args => Math.Clamp(args[0].Solve(), args[1].Solve(), args[2].Solve()),
		["sign"] = args => Math.Sign(args[0].Solve()),
		["cpsign"] = args => Math.CopySign(args[0].Solve(), args[1].Solve()),
		#endregion

		#region Bits
		["bitinc"] = args => Math.BitIncrement(args[0].Solve()),
		["bitdec"] = args => Math.BitDecrement(args[0].Solve()),
		#endregion

		#region ListProperties
		["max"] = args => ListHandler.Solve(args, x => x.Max()),
		["min"] = args => ListHandler.Solve(args, x => x.Min()),
		["sum"] = args => ListHandler.Solve(args, x => x.Sum()),
		["product"] = args => ListHandler.Solve(args, x => x.Product()),
		["mean"] = args => ListHandler.Solve(args, x => x.Average()),
		["average"] = args => ListHandler.Solve(args, x => x.Average()),
		["median"] = args => ListHandler.Solve(args, x => x.Median()),
		["mode"] = args => ListHandler.Solve(args, x => x.Mode()),
		#endregion

		#region ListMemory
		["len"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"len\" requires a pointer node as its first argument.");

			return ListHandler.Length(pointer);
		},
		["get"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"get\" requires a pointer node as its first argument.");

			return ListHandler.Get(pointer, args[1].Solve());
		},
		["set"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"set\" requires a pointer node as its first argument.");

			return ListHandler.Set(pointer, args[1].Solve(), args[2].Solve());
		},
		["add"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"add\" requires a pointer node as its first argument.");

			return ListHandler.Add(pointer, args[1].Solve());
		},
		["indexof"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"indexof\" requires a pointer node as its first argument.");

			return ListHandler.IndexOf(pointer, args[1].Solve());
		},
		["remove"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"remove\" requires a pointer node as its first argument.");

			return ListHandler.Remove(pointer, args[1].Solve());
		},
		["clear"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"clear\" requires a pointer node as its first argument.");

			return ListHandler.Clear(pointer);
		},
		["contains"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"contains\" requires a pointer node as its first argument.");

			return ListHandler.Enumerate(pointer).Contains(args[1].Solve()) ? 1 : 0;
		},
		["concat"] = args =>
		{
			PointerNode a = args[0] as PointerNode ?? throw new WingCalcException("Function \"concat\" requires a pointer node as its first argument.");
			PointerNode b = args[1] as PointerNode ?? throw new WingCalcException("Function \"concat\" requires a pointer node as its second argument.");
			PointerNode c = args.Count >= 3
								? args[2] as PointerNode ?? throw new WingCalcException("Function \"concat\" requires a pointer node as its third argument.")
								: a;

			return ListHandler.Concat(a, b, c);
		},
		["copy"] = args =>
		{
			PointerNode a = args[0] as PointerNode ?? throw new WingCalcException("Function \"copy\" requires a pointer node as its first argument.");
			PointerNode b = args[1] as PointerNode ?? throw new WingCalcException("Function \"copy\" requires a pointer node as its second argument.");

			return ListHandler.Copy(a, b);
		},
		["setify"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"setify\" requires a pointer node as its first argument.");

			return ListHandler.Setify(pointer);
		},
		["stringify"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"stringify\" requires a pointer node as its first argument.");

			return ListHandler.Stringify(pointer);
		},
		["sort"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"sort\" requires a pointer node as its first argument.");

			return ListHandler.Allocate(pointer, ListHandler.Enumerate(pointer).OrderBy(x => x).ToList());
		},
		["iter"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"iter\" requires a pointer node as its first argument.");
			IAssignable lambdaVar = args[1] as IAssignable ?? throw new WingCalcException("Function \"iter\" requires an assignable node as its second argument.");

			if (args.Count >= 4)
			{
				IAssignable indexVar = args[2] as IAssignable ?? throw new WingCalcException("Function \"iter\" requires an assignable node as its third argument.");
				foreach (var (x, i) in ListHandler.Enumerate(pointer).Select((x, i) => (x, i)))
				{
					lambdaVar.Assign(x);
					indexVar.Assign(i);
					args[3].Solve();
				}
			}
			else
			{
				foreach (double x in ListHandler.Enumerate(pointer))
				{
					lambdaVar.Assign(x);
					args[2].Solve();
				}
			}

			return ListHandler.Enumerate(pointer).Count();
		},
		["filter"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"filter\" requires a pointer node as its first argument.");
			IAssignable lambdaVar = args[1] as IAssignable ?? throw new WingCalcException("Function \"filter\" requires an assignable node as its second argument.");

			List<double> filtered = new();

			if (args.Count >= 4)
			{
				IAssignable indexVar = args[2] as IAssignable ?? throw new WingCalcException("Function \"filter\" requires an assignable node as its third argument.");
				foreach (var (x, i) in ListHandler.Enumerate(pointer).Select((x, i) => (x, i)))
				{
					lambdaVar.Assign(x);
					indexVar.Assign(i);

					if (args[3].Solve() != 0)
					{
						filtered.Add(x);
					}
				}
			}
			else
			{
				foreach (double x in ListHandler.Enumerate(pointer))
				{
					lambdaVar.Assign(x);

					if (args[2].Solve() != 0)
					{
						filtered.Add(x);
					}
				}
			}

			ListHandler.Allocate(pointer, filtered);

			return ListHandler.Enumerate(pointer).Count();
		},
		["any"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"any\" requires a pointer node as its first argument.");
			IAssignable lambdaVar = args[1] as IAssignable ?? throw new WingCalcException("Function \"any\" requires an assignable node as its second argument.");

			if (args.Count >= 4)
			{
				IAssignable indexVar = args[2] as IAssignable ?? throw new WingCalcException("Function \"any\" requires an assignable node as its third argument.");
				foreach (var (x, i) in ListHandler.Enumerate(pointer).Select((x, i) => (x, i)))
				{
					lambdaVar.Assign(x);
					indexVar.Assign(i);

					if (args[3].Solve() != 0)
					{
						return 1;
					}
				}
			}
			else
			{
				foreach (double x in ListHandler.Enumerate(pointer))
				{
					lambdaVar.Assign(x);

					if (args[2].Solve() != 0)
					{
						return 1;
					}
				}
			}

			return 0;
		},
		["count"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"count\" requires a pointer node as its first argument.");
			IAssignable lambdaVar = args[1] as IAssignable ?? throw new WingCalcException("Function \"count\" requires an assignable node as its second argument.");

			int count = 0;

			if (args.Count >= 4)
			{
				IAssignable indexVar = args[2] as IAssignable ?? throw new WingCalcException("Function \"count\" requires an assignable node as its third argument.");
				foreach (var (x, i) in ListHandler.Enumerate(pointer).Select((x, i) => (x, i)))
				{
					lambdaVar.Assign(x);
					indexVar.Assign(i);

					if (args[3].Solve() != 0)
					{
						count++;
					}
				}
			}
			else
			{
				foreach (double x in ListHandler.Enumerate(pointer))
				{
					lambdaVar.Assign(x);

					if (args[2].Solve() != 0)
					{
						count++;
					}
				}
			}

			return count;
		},
		#endregion

		#region ControlFlow
		["if"] = args => args[0].Solve() != 0 ? args[1].Solve() : args[2].Solve(),
		["for"] = args =>
		{
			args[0].Solve();
			int count = 0;

			while (args[1].Solve() != 0)
			{
				args[3].Solve();
				args[2].Solve();
				count++;
			}

			return count;
		},
		["while"] = args =>
		{
			int count = 0;

			while (args[0].Solve() != 0)
			{
				args[1].Solve();
				count++;
			}

			return count;
		},
		["dowhile"] = args =>
		{
			int count = 0;

			do
			{
				args[1].Solve();
				count++;
			}
			while (args[0].Solve() != 0);

			return count;
		},
		["repeat"] = args =>
		{
			double count = args[1].Solve();

			for (int i = 0; i < count; i++)
			{
				args[0].Solve();
			}

			return count;
		},
		#endregion

		#region Memory
		["alloc"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"alloc\" requires a pointer node as its first argument.");

			List<double> values = args.Skip(1).Select(x => x.Solve()).ToList();

			return ListHandler.Allocate(pointer, values);
		},
		["salloc"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"salloc\" requires a pointer node as its first argument.");
			QuoteNode quote = args[1] as QuoteNode ?? throw new WingCalcException("Function \"salloc\" requires a quote node as its second argument.");

			return ListHandler.StringAllocate(pointer, quote.Text.Select(x => (double)x).ToList());
		},
		["range"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"alloc\" requires a pointer node as its first argument.");

			double start, end;

			if (args.Count >= 3)
			{
				start = args[1].Solve();
				end = args[2].Solve();
			}
			else
			{
				start = 0;
				end = args[1].Solve();
			}

			List<double> vals = new();
			for (double i = start; i < end; i++)
			{
				vals.Add(i);
			}

			return ListHandler.Allocate(pointer, vals);
		},
		["memprint"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"memprint\" requires a pointer node as its first argument.");

			Solver solver = pointer.Solver;
			double address = pointer.Address;
			double value = solver.GetVariable(address.ToString());

			solver.WriteLine($"${address} = {value}");
			return value;
		},
		["print"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"print\" requires a pointer node as its first argument.");

			pointer.Solver.WriteLine($"{{ {string.Join(", ", ListHandler.Enumerate(pointer))} }}");

			return ListHandler.Length(pointer);
		},
		#endregion

		#region Strings
		["exec"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"exec\" requires a pointer node as its first argument.");
			StringBuilder sb = new();

			Solver solver = pointer.Solver;
			double address = pointer.Address;

			for (int i = 0; true; i++)
			{
				char found = (char)solver.GetVariable((address + i).ToString());

				if (found == '\0') break;
				else sb.Append(found);
			}

			return solver.Solve(sb.ToString());
		},
		["listify"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"listify\" requires a pointer node as its first argument.");

			return ListHandler.Listify(pointer);
		},
		["slen"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"slen\" requires a pointer node as its first argument.");

			return ListHandler.StringEnumerate(pointer).Count();
		},
		["siter"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"siter\" requires a pointer node as its first argument.");
			IAssignable lambdaVar = args[1] as IAssignable ?? throw new WingCalcException("Function \"siter\" requires an assignable node as its second argument.");

			if (args.Count >= 4)
			{
				IAssignable indexVar = args[2] as IAssignable ?? throw new WingCalcException("Function \"siter\" requires an assignable node as its third argument.");
				foreach (var (x, i) in ListHandler.StringEnumerate(pointer).Select((x, i) => (x, i)))
				{
					lambdaVar.Assign(x);
					indexVar.Assign(i);
					args[3].Solve();
				}
			}
			else
			{
				foreach (double x in ListHandler.StringEnumerate(pointer))
				{
					lambdaVar.Assign(x);
					args[2].Solve();
				}
			}

			return ListHandler.StringEnumerate(pointer).Count();
		},
		#endregion

		#region Factors
		["gcd"] = args => ListHandler.Solve(args, x => (double)x.Aggregate((a, b) => (double)Factorizer.GCD((BigInteger)a, (BigInteger)b))),
		["lcm"] = args => ListHandler.Solve(args, x => (double)x.Aggregate((a, b) => (double)Factorizer.LCM((BigInteger)a, (BigInteger)b))),
		["factor"] = args =>
		{
			List<double> factors;

			if (args[0] is PointerNode pointer)
			{
				factors = Factorizer.Factors((BigInteger)args[1].Solve()).Select(x => (double)x).ToList();

				ListHandler.Allocate(pointer, factors);
			}
			else
			{
				factors = Factorizer.Factors((BigInteger)args[0].Solve()).Select(x => (double)x).ToList();

				Solver.WriteLine($"{{ {string.Join(", ", factors)} }}");
			}

			return factors.Count;
		},
		["prime"] = args => Factorizer.IsPrime((BigInteger)args[0].Solve()) ? 1 : 0,
		["primefactor"] = args =>
		{
			List<double> factors;

			if (args[0] is PointerNode pointer)
			{
				factors = Factorizer.PrimeFactors((BigInteger)args[1].Solve()).Select(x => (double)x).ToList();

				ListHandler.Allocate(pointer, factors);
			}
			else
			{
				factors = Factorizer.PrimeFactors((BigInteger)args[0].Solve()).Select(x => (double)x).ToList();

				Solver.WriteLine($"{{ {string.Join(", ", factors)} }}");
			}

			return factors.Count;
		},
		#endregion

		#region Programming
		["eval"] = args => args[0].Solve(),
		["assign"] = args =>
		{
			IAssignable assignable;

			try
			{
				assignable = ((AssignmentNode)args[0]).A;
			}
			catch
			{
				throw new WingCalcException($"The first argument of the \"msum\" must be an assignment.");
			}

			return assignable.Assign(args[1].Solve());
		},
		["ignore"] = args => 0,
		#endregion

		#region Random
		["rand"] = args =>
		{
			if (args.Count == 0) return _random.NextDouble();
			else if (args.Count == 1) return _random.Next((int)args[0].Solve());
			else return _random.Next((int)args[0].Solve(), (int)args[1].Solve());
		},
		["srand"] = args =>
		{
			int x = (int)args[0].Solve();
			_random = new(x);
			return x;
		}
		#endregion
	};

	internal static Func<List<INode>, double> Get(string s) => _functions[s];

	private static long FactorialDivision(int x, int y)
	{
		if (x <= 0) throw new WingCalcException("Cannot compute permutation/combination with non-positive n.");
		if (y < 0) throw new WingCalcException("Cannot compute permutation/combination with negative n - k.");
		if (y > x) throw new WingCalcException("Cannot compute permutation/combination with k > n");

		long result = 1;
		for (int i = x; i > y; i--)
		{
			result *= i;
		}

		return result;
	}

	private static long Factorial(int x)
	{
		if (x < 0) throw new WingCalcException("Cannot calculate the factorial of a negative number.");

		long result = 1;

		for (int i = x; i > 1; i--) result *= i;

		return result;
	}
}
