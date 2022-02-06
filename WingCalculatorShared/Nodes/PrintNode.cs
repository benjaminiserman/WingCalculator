namespace WingCalculatorShared;
using System;

internal record PrintNode(INode A, INode B, Solver Solver, bool Newline) : INode
{
	public double Solve()
	{
		double a = A.Solve();
		double b = B.Solve();

		if (A is QuoteNode qn && b == 1)
		{
			if (Newline) Solver.WriteLine(qn.Text);
			else Solver.Write(qn.Text);
		}
		else
		{
			if (Newline) Solver.WriteLine(GetPrint(a, b));
			else Solver.Write(GetPrint(a, b));
		}

		return a;
	}

	private static string GetPrint(double x, double y) => y switch
	{
		-1 => "\n",
		0 => x.ToString(),
		1 => ((char)(int)x).ToString(),
		2 => Convert.ToString((int)x, 2),
		8 => Convert.ToString((int)x, 8),
		16 => Convert.ToString((int)x, 16),

		_ => throw new NotImplementedException($"Format {y} is not implemented.")
	};
}