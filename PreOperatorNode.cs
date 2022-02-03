namespace Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class PreOperatorNode : INode
{
	public static readonly int MaxTier = 10;

	public string Text { get; private set; }

	public PreOperatorNode(string s) => Text = s;

	public double Solve() => throw new NotImplementedException();

	public int Tier => Text switch
	{
		"*" => 0,
		"/" => 0,
		"%" => 0,
		"//" => 0,

		"+" => 1,
		"-" => 1,

		"<<" => 2,
		">>" => 2,

		"<" => 3,
		">" => 3,
		"<=" => 3,
		">=" => 3,

		"==" => 4,
		"!=" => 4,

		"&" => 5,

		"^" => 6,

		"|" => 7,

		"&&" => 8,

		"||" => 9,

		"*=" => 10,
		"/=" => 10,
		"%=" => 10,
		"//=" => 10,
		"+=" => 10,
		"-=" => 10,
		"<<=" => 10,
		">>=" => 10,
		"&=" => 10,
		"^=" => 10,
		"|=" => 10,
		"=" => 10,

		_ => throw new NotImplementedException()
	};
}
