namespace WingCalculatorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal static class Functions
{
	private static readonly Dictionary<string, Func<List<INode>, double>> _functions = new()
	{
		#region Exponential
		["pow"] = args => Math.Pow(args[0].Solve(), args[1].Solve()),
		["exp"] = args => Math.Exp(args[0].Solve()),
		["sqrt"] = args => Math.Sqrt(args[0].Solve()),
		["cbrt"] = args => Math.Cbrt(args[0].Solve()),

		["log"] = args => Math.Log(args[0].Solve(), args[1].Solve()),
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

		["abs"] = args => Math.Abs(args[0].Solve()),
		["clamp"] = args => Math.Clamp(args[0].Solve(), args[1].Solve(), args[2].Solve()),
		["sign"] = args => Math.Sign(args[0].Solve()),
		["cpsign"] = args => Math.CopySign(args[0].Solve(), args[1].Solve()),

		["bitinc"] = args => Math.BitIncrement(args[0].Solve()),
		["bitdec"] = args => Math.BitDecrement(args[0].Solve()),

		#region List
		["max"] = args => args.Select(x => x.Solve()).Max(),
		["min"] = args => args.Select(x => x.Solve()).Min(),
		["sum"] = args => args.Select(x => x.Solve()).Sum(),
		["product"] = args => args.Aggregate((x, y) => new ConstantNode(x.Solve() * y.Solve())).Solve(),
		["mean"] = args => args.Select(x => x.Solve()).Average(),
		["median"] = args =>
		{
			List<double> solved = args.Select(x => x.Solve()).ToList();
			solved.Sort();

			return solved.Count % 2 == 0
				? solved.GetRange(solved.Count / 2 - 1, 2).Average()
				: solved[solved.Count / 2];
		},
		["mode"] = args => args.Select(x => x.Solve()).GroupBy(v => v).OrderByDescending(g => g.Count()).First().Key,
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
			PointerNode pointer = (PointerNode)args[0];

			Solver solver = pointer.Solver;
			double address = pointer.Address;

			solver.SetVariable(address.ToString(), args.Count - 1);

			for (int i = 1; i < args.Count; i++)
			{
				solver.SetVariable((address + i).ToString(), args[i].Solve());
			}

			return address;
		},
		["salloc"] = args =>
		{
			PointerNode pointer = (PointerNode)args[0];
			QuoteNode quote = (QuoteNode)args[1];

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
			PointerNode pointer = (PointerNode)args[0];

			Solver solver = pointer.Solver;
			double address = pointer.Address;
			double value = solver.GetVariable(address.ToString());

			solver.WriteLine($"${address} = {value}");
			return value;
		},
		#endregion

		#region Strings
		["exec"] = args =>
		{
			PointerNode pointer = (PointerNode)args[0];
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
	};

	internal static Func<List<INode>, double> Get(string s) => _functions[s];
}
