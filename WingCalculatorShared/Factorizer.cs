namespace WingCalculatorShared;
using System;
using System.Collections.Generic;
using System.Numerics;

internal static class Factorizer
{
	public static BigInteger GCD(BigInteger x, BigInteger y) => y == 0 ? x : GCD(y, x % y);

	public static BigInteger LCM(BigInteger x, BigInteger y)
	{
		if (x == y && x == 0) return 0;

		return BigInteger.Abs(x * y) / GCD(x, y);
	}

	public static List<BigInteger> PrimeFactors(BigInteger x)
	{
		List<BigInteger> factors = new();

		if (x == 0) return new();

		int max = (int)Math.Ceiling(Math.Sqrt((double)x));

		while (x % 2 == 0)
		{
			factors.Add(2);
			x /= 2;
		}

		for (int i = 3; i < max; i += 2)
		{
			while (x % i == 0)
			{
				factors.Add(i);
				x /= i;
			}
		}

		if (factors.Count == 0)
		{
			factors.Add(1);
		}

		if (x != 1)
		{
			factors.Add(x);
		}

		return factors;
	}

	public static List<BigInteger> Factors(BigInteger x)
	{
		HashSet<BigInteger> factors = new();

		if (x == 0) return new();

		int max = (int)Math.Ceiling(Math.Sqrt((double)x));

		for (int i = 1; i <= max; i++)
		{
			if (x % i == 0)
			{
				factors.Add(i);
				factors.Add(x / i);
			}
		}

		var list = factors.ToList();
		list.Sort();

		return list;
	}

	public static bool IsPrime(BigInteger x) => (Factors(x)?.Count ?? 0) == 2;
}
