namespace WingCalculatorShared;

internal static class RomanNumeralConverter
{
	private static readonly Dictionary<char, double> _numerals = new()
	{
		['N'] = 0,
		['I'] = 1,
		['V'] = 5,
		['X'] = 10,
		['L'] = 50,
		['C'] = 100,
		['D'] = 500,
		['M'] = 1000,
		['·'] = 1.0/12,
		[':'] = 2.0/12,
		['∴'] = 3.0 / 12,
		['∷'] = 4.0 / 12,
		['⁙'] = 5.0 / 12,
		['S'] = 1.0 / 2,
		['℈'] = 1.0 / 288,
		['Ↄ'] = 1.0 / 48,
		['Σ'] = 1.0 / 24,
		['Є'] = 1.0 / 24,
		['A'] = 5,
		['ↅ'] = 6,
		['Z'] = 7,
		['O'] = 11,
		['F'] = 40,
		['R'] = 80,
		['Y'] = 150,
		['K'] = 151,
		['T'] = 160,
		['H'] = 200,
		['E'] = 250,
		['B'] = 300,
		['P'] = 400,
		['G'] = 400,
		['Q'] = 500,
		['Ω'] = 800,
		['Ϡ'] = 900,
	};

	public static double GetValue(string s)
	{
		double value = 0;
		Numeral lastNumeral = Numeral.Zero;
		int num = 0;

		if (s == "N") return 0;
		else if (s.Contains('N')) throw new Exceptions.WingCalcException("Roman numerals cannot contain place-holding zeros (\'N\').");

		List<Numeral> numerals = Preprocess(s);

		foreach (Numeral numeral in numerals)
		{
			if (numeral == lastNumeral) num++;
			else
			{
				if (lastNumeral == Numeral.Zero)
				{
					lastNumeral = numeral;
					num = 1;
				}
				else if (numeral.Value < lastNumeral.Value)
				{
					value += num * lastNumeral.Value;
					lastNumeral = numeral;
					num = 1;
				}
				else
				{
					value += numeral.Value - num * lastNumeral.Value;
					lastNumeral = Numeral.Zero;
					num = 0;
				}
			}
		}

		value += num * lastNumeral.Value;

		return value;
	}

	private static List<Numeral> Preprocess(string s)
	{
		List<Numeral> list = new();

		for (int i = 0; i < s.Length; i++)
		{
			char c = s[i];
			int exponent = 0;

			for (i++; i < s.Length; i++)
			{
				if (s[i] == '̅') exponent++;
				else
				{
					i--;
					break;
				}
			}

			list.Add(new(c, exponent));
		}

		return list;
	}

	public static bool IsNumeral(char c) => _numerals.ContainsKey(char.ToUpper(c));

	private record struct Numeral
	{
		public char character;
		public int exponent;

		public Numeral(char character, int exponent)
		{
			this.character = char.ToUpperInvariant(character);
			this.exponent = exponent;
		}

		public double Value => _numerals[character] * Math.Pow(1000, exponent);

		public static Numeral Zero => new('N', 0);
	}
}