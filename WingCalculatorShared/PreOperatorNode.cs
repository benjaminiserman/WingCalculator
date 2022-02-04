namespace WingCalculatorShared;
using System;

internal class PreOperatorNode : INode
{
	public static readonly int MaxTier = 15;

	public string Text { get; private set; }

	public PreOperatorNode(string s) => Text = s;

	public double Solve() => throw new NotImplementedException();

	public int Tier => Text switch
	{
		"?" => 0,

		"**" => 1,

		"*" => 2,
		"/" => 2,
		"%" => 2,
		"//" => 2,

		"+" => 3,
		"-" => 3,

		"<<" => 4,
		">>" => 4,

		"<" => 5,
		">" => 5,
		"<=" => 5,
		">=" => 5,
		"<?" => 5,
		">?" => 5,
		"<=?" => 5,
		">=?" => 5,

		"==" => 6,
		"!=" => 6,

		"&" => 7,

		"^" => 8,

		"|" => 9,

		"&&" => 10,

		"||" => 11,

		"?:" => 12,

		"**=" => 13,
		"*=" => 13,
		"/=" => 13,
		"%=" => 13,
		"//=" => 13,
		"+=" => 13,
		"-=" => 13,
		"<<=" => 13,
		">>=" => 13,
		"&=" => 13,
		"^=" => 13,
		"|=" => 13,
		"?=" => 13,
		"=" => 13,

		":" => 14,

		";" => 15,

		_ => throw new NotImplementedException()
	};
}
