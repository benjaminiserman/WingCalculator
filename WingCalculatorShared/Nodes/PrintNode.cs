namespace WingCalculatorShared.Nodes;
using System;

internal record PrintNode(INode A, INode B, Solver Solver, bool Newline) : INode
{
	public double Solve()
	{
		double a = A.Solve();
		double b = B.Solve();

		if (Newline) Solver.WriteLine(GetPrint(a, b));
		else Solver.Write(GetPrint(a, b));

		return a;
	}

	private static string GetPrint(double x, double y) => y switch
	{
		-1 => "\n",
		0 => x.ToString(),
		1 => ((char)(int)x).ToString(),
		2 => Convert.ToString((int)x, 2),
		3 => $"{x:P}",
		4 => GetFraction(x),
		5 => $"{x:F}",
		8 => Convert.ToString((int)x, 8),
		16 => Convert.ToString((int)x, 16),
		Math.PI => $"{x / Math.PI}π",

		_ => throw new NotImplementedException($"Format {y} is not implemented.")
	};

	public static string Documentation => "Prints its left-hand operand to standard output, as specified by its right-hand operand.\n" +
		"Valid Formats:\n" +
		" [-1 / $NL] => prints a newline\n" +
		" [0 / $DEC] => prints the left-hand operand\n" +
		" [1 / $TXT] => prints the left-hand operand converted to a UTF-16 character\n" +
		" [2 / $BIN] => prints the left-hand operand casted to an integer and converted into binary\n" +
		" [3 / $PCT] => prints the left-hand operand as a percentage\n" +
		" [4 / $FRAC] => prints the left-hand operand as a fraction accurate to within 1e-20\n" +
		" [5 / $EXACT] => prints the left-hand operand as an exact decimal value\n" +
		" [8 / $OCT] => prints the left-hand operand casted to an integer and converted into octal\n" +
		" [16 / $HEX] => prints the left-hand operand casted to an integer and converted into hexadecimal\n" +
		"Note: Each pair format string [X / $Y] pair is provided as a variable $Y and its default value X.\n" +
		"PrintNodes choose a format specifier based on *the default value* of $Y, so if $Y is changed, it may cause unexpected behavior when used as a format specifier.";

	// adapted from the Stern-Brocot tree traversal algorithm described here: https://stackoverflow.com/questions/5124743/algorithm-for-simplifying-decimal-to-fractions
	private static string GetFraction(double x, double error = 1e-20)
	{
		int n = (int)Math.Floor(x);

		x -= n;

		if (x < error) return $"{n}";
		else if (1 - error < x) return $"{n + 1}";

		int lowerN = 0;
		int lowerD = 1;

		int upperN = 1;
		int upperD = 1;

		while (true)
		{
			int middleN = lowerN + upperN;
			int middleD = lowerD + upperD;

			if (middleD * (x + error) < middleN)
			{
				upperN = middleN;
				upperD = middleD;
			}
			else if (middleN < (x - error) * middleD)
			{
				lowerN = middleN;
				lowerD = middleD;
			}
			else
			{
				if (n == 0) return $"{middleN} / {middleD}";
				else return $"{n * middleD + middleN} / {middleD}\nOR\n{n} + ({middleN} / {middleD})";
			}
		}
	}
}