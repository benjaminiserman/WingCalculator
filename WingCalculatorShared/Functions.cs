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

			IAssignable assignable = ((AssignmentNode)args[0]).A;
			INode counterNode = (INode)assignable;

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

			IAssignable assignable = ((AssignmentNode)args[0]).A;
			INode counterNode = (INode)assignable;

			while (counterNode.Solve() <= end)
			{
				product *= args[2].Solve();
				assignable.Assign(counterNode.Solve() + 1);
			}

			return product;
		},
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

		#region List
		["max"] = args => ListHandler.Solve(args, x => x.Max()),
		["min"] = args => ListHandler.Solve(args, x => x.Min()),
		["sum"] = args => ListHandler.Solve(args, x => x.Sum()),
		["product"] = args => ListHandler.Solve(args, x => x.Product()),
		["mean"] = args => ListHandler.Solve(args, x => x.Average()),
		["average"] = args => ListHandler.Solve(args, x => x.Average()),
		["median"] = args => ListHandler.Solve(args, x => x.Median()),
		["mode"] = args => ListHandler.Solve(args, x => x.Mode()),
		#endregion

		#region ControlFlow
		["if"] = args => args[0].Solve() != 0 ? args[1].Solve() : args[2].Solve(),
		["range"] = args =>
		{
			int i = (int)args[0].Solve();
			int end = (int)args[1].Solve();
			int count = 0;

			if (i < end)
			{
				while (i < end)
				{
					args[2].Solve();
					count++;
					i++;
					if (args[0] is IAssignable ia) ia.Assign(i);
				}
			}
			else if (i > end)
			{
				while (i > end)
				{
					args[2].Solve();
					count++;
					i--;
					if (args[0] is IAssignable ia) ia.Assign(i);
				}
			}

			return count;
		},
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
		#endregion

		#region Memory
		["alloc"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"alloc\" requires a pointer node as its first argument.");

			List<double> values = args.Skip(1).Select(x => x.Solve()).ToList();

			ListHandler.Allocate(pointer, values);

			return pointer.Address;
		},
		["salloc"] = args =>
		{
			PointerNode pointer = args[0] as PointerNode ?? throw new WingCalcException("Function \"salloc\" requires a pointer node as its first argument.");
			QuoteNode quote = args[1] as QuoteNode ?? throw new WingCalcException("Function \"salloc\" requires a quote node as its second argument.");

			Solver solver = pointer.Solver;
			double address = pointer.Address;

			for (int i = 0; i < quote.Text.Length; i++)
			{
				solver.SetVariable((address + i).ToString(), quote.Text[i]);
			}

			solver.SetVariable((address + quote.Text.Length).ToString(), 0);

			return address;
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
		["prime"] = args =>
		{
			BigInteger x = (BigInteger)args[0].Solve();

			return Factorizer.Factors(x).Count == 2 ? 1 : 0;
		},
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
