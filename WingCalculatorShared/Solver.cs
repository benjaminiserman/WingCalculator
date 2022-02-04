namespace WingCalculatorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Solver
{
	private readonly Dictionary<string, double> _variables = new()
	{
		["PI"] = Math.PI,
		["TAU"] = Math.Tau,
		["E"] = Math.E,

		["BYTEMIN"] = byte.MinValue,
		["BYTEMAX"] = byte.MaxValue,
		["SBYTEMIN"] = sbyte.MinValue,
		["SBYTEMAX"] = sbyte.MaxValue,
		["SHORTMIN"] = short.MinValue,
		["SHORTMAX"] = short.MaxValue,
		["USHORTMIN"] = ushort.MinValue,
		["USHORTMAX"] = ushort.MaxValue,
		["INTMIN"] = int.MinValue,
		["INTMAX"] = int.MaxValue,
		["UINTMIN"] = uint.MinValue,
		["UINTMAX"] = uint.MaxValue,
		["LONGMIN"] = long.MinValue,
		["LONGMAX"] = long.MaxValue,
		["ULONGMIN"] = ulong.MinValue,
		["ULONGMAX"] = ulong.MaxValue,

		["DOUBLEMIN"] = double.MinValue,
		["DOUBLEMAX"] = double.MaxValue,
		["INFINITY"] = double.PositiveInfinity,
		["EPSILON"] = double.Epsilon,
		["NAN"] = double.NaN,

		["DEC"] = 0,
		["ASCII"] = 1,
		["BIN"] = 2,
		["OCT"] = 8,
		["HEX"] = 16,

		["ANS"] = 0,
	};

	private readonly Dictionary<string, Func<List<double>, double>> _functions = new()
	{
		["pow"] = args => Math.Pow(args[0], args[1]),
		["exp"] = args => Math.Exp(args[0]),
		["sqrt"] = args => Math.Sqrt(args[0]),
		["cbrt"] = args => Math.Cbrt(args[0]),

		["ceil"] = args => Math.Ceiling(args[0]),
		["floor"] = args => Math.Floor(args[0]),
		["round"] = args => Math.Round(args[0], args.Count > 1 ? (int)args[1] : 0, (MidpointRounding)(args.Count > 2 ? (int)args[2] : 0)),
		["trunc"] = args => (int)args[0],

		["sin"] = args => Math.Sin(args[0]),
		["cos"] = args => Math.Cos(args[0]),
		["tan"] = args => Math.Tan(args[0]),
		["asin"] = args => Math.Asin(args[0]),
		["acos"] = args => Math.Acos(args[0]),
		["atan"] = args => args.Count > 1 ? Math.Atan2(args[0], args[1]) : Math.Atan(args[0]),
		["sinh"] = args => Math.Sinh(args[0]),
		["cosh"] = args => Math.Cosh(args[0]),
		["tanh"] = args => Math.Tanh(args[0]),
		["arcsinh"] = args => Math.Asinh(args[0]),
		["arccosh"] = args => Math.Acosh(args[0]),
		["arctanh"] = args => Math.Atanh(args[0]),

		["abs"] = args => Math.Abs(args[0]),
		["clamp"] = args => Math.Clamp(args[0], args[1], args[2]),
		["sign"] = args => Math.Sign(args[0]),
		["cpsign"] = args => Math.CopySign(args[0], args[1]),

		["bitinc"] = args => Math.BitIncrement(args[0]),
		["bitdec"] = args => Math.BitDecrement(args[0]),

		["max"] = args => args.Max(),
		["min"] = args => args.Min(),
		["sum"] = args => args.Sum(),
		["product"] = args => args.Aggregate((x, y) => x * y),
		["mean"] = args => args.Average(),
		["median"] = args =>
		{
			args.Sort();

			return args.Count % 2 == 0
				? args.GetRange(args.Count / 2 - 1, 2).Average()
				: args[args.Count / 2];
		},
		["mode"] = args => args.GroupBy(v => v).OrderByDescending(g => g.Count()).First().Key,

		["log"] = args => Math.Log(args[0], args[1]),
		["ln"] = args => Math.Log(args[0]),
	};

	private readonly Dictionary<string, INode> _macros = new();

	public Action<string> WriteLine { get; set; } = Console.WriteLine;
	public Action<string> WriteError { get; set; } = Console.WriteLine;
	public Action<string> Write { get; set; } = Console.Write;
	public Func<string> ReadLine { get; set; } = Console.ReadLine;

	public double Solve(string s, bool setAns = true)
	{
		var tokens = Tokenizer.Tokenize(s).ToArray();

		INode node = CreateTree(tokens);

		double solve = node.Solve();

		if (setAns) SetVariable("ANS", solve);

		return solve;
	}

	private INode CreateTree(Span<Token> tokens)
	{
		List<INode> availableNodes = new();
		bool isCoefficient = false;

		for (int i = 0; i < tokens.Length; i++)
		{
			switch (tokens[i].TokenType)
			{
				case TokenType.Number:
				{
					availableNodes.Add(new ConstantNode(double.Parse(tokens[i].Text)));
					isCoefficient = true;
					break;
				}
				case TokenType.Operator:
				{
					availableNodes.Add(new PreOperatorNode(tokens[i].Text));
					isCoefficient = false;
					break;
				}
				case TokenType.Function:
				{
					if (tokens[i + 1].TokenType != TokenType.OpenParen) throw new Exception("Function called but no opening parenthesis found!");

					int end = FindClosing(i + 1, tokens);

					INode tree = new FunctionNode(tokens[i].Text, this, CreateParams(tokens[(i + 2)..end]));

					if (isCoefficient)
					{
						INode coefficientNode = availableNodes[^1];
						availableNodes.Remove(coefficientNode);
						availableNodes.Add(OperatorNodeFactory.CreateBinaryNode(coefficientNode, new("*"), tree));
					}
					else availableNodes.Add(tree);
					
					i = end;
					isCoefficient = false;
					break;
				}
				case TokenType.Hex:
				{
					availableNodes.Add(new ConstantNode(Convert.ToInt32(tokens[i].Text[1..], 16)));
					isCoefficient = true;
					break;
				}
				case TokenType.OpenParen: // $$$ add paren multiplication
				{
					int end = FindClosing(i, tokens);

					INode tree = CreateTree(tokens[(i + 1)..end]);

					if (isCoefficient)
					{
						INode coefficientNode = availableNodes[^1];
						availableNodes.Remove(coefficientNode);
						availableNodes.Add(OperatorNodeFactory.CreateBinaryNode(coefficientNode, new("*"), tree));
					}
					else availableNodes.Add(tree);

					i = end;
					isCoefficient = true;
					break;
				}
				case TokenType.CloseParen:
				{
					throw new Exception("Dangling closing parenthesis found!");
				}
				case TokenType.Comma:
				{
					throw new Exception("Dangling comma found!");
				}
				case TokenType.Variable:
				{
					if (isCoefficient)
					{
						availableNodes.Add(new PreOperatorNode("*"));
					}

					if (tokens[i].Text.Length == 1) availableNodes.Add(new PreOperatorNode("$"));
					else availableNodes.Add(new VariableNode(tokens[i].Text[1..], this));

					isCoefficient = true;
					break;
				}
				case TokenType.Macro:
				{
					if (tokens[i].Text.Length == 1) availableNodes.Add(new PreOperatorNode("@"));
					else availableNodes.Add(new MacroNode(tokens[i].Text[1..], this));

					isCoefficient = false;
					break;
				}
				case TokenType.Quote:
				{
					availableNodes.Add(new QuoteNode(Regex.Unescape(tokens[i].Text), this));
					isCoefficient = true;
					break;
				}
				case TokenType.Char:
				{
					string unescaped = Regex.Unescape(tokens[i].Text);
					if (unescaped.Length > 1) throw new Exception($"Character '{tokens[i].Text}' could not be resolved: too many characters.");
					availableNodes.Add(new ConstantNode(unescaped[0]));
					isCoefficient = true;
					break;
				}
				case TokenType.Binary:
				{
					availableNodes.Add(new ConstantNode(Convert.ToInt32(tokens[i].Text, 2)));
					isCoefficient = true;
					break;
				}
			}
		}

		if (availableNodes[0] is PreOperatorNode firstNode && (firstNode.Text.Length >= 2 || !Tokenizer._unaryOperators.Contains(firstNode.Text[0]))) availableNodes.Insert(0, new VariableNode("ANS", this)); // add $ANS at when start with binary operator

		if (availableNodes.Count > 0 && availableNodes[^1] is PreOperatorNode semiNode && semiNode.Text == ";") availableNodes.RemoveAt(availableNodes.Count - 1); // remove trailing semicolons

		for (int i = 1; i < availableNodes.Count; i++) // handle unary operators
		{
			if (availableNodes[i] is not PreOperatorNode
				&& availableNodes[i - 1] is PreOperatorNode signNode 
				&& (i == 1 || availableNodes[i - 2] is PreOperatorNode))
			{
				INode numberNode = availableNodes[i];

				if (Tokenizer._unaryOperators.Contains(signNode.Text))
				{
					availableNodes.RemoveAt(i - 1);
					switch (signNode.Text)
					{
						case "+":
						{
							break;
						}
						case "-":
						{
							availableNodes.RemoveAt(i - 1);
							availableNodes.Insert(i - 1, new UnaryNode(numberNode, x => -x));
							break;
						}
						case "$":
						{
							availableNodes.RemoveAt(i - 1);
							availableNodes.Insert(i - 1, new PointerNode(numberNode, this));
							break;
						}
						case "@":
						{
							availableNodes.RemoveAt(i - 1);
							availableNodes.Insert(i - 1, new MacroPointerNode(numberNode, this));
							break;
						}
						case "!":
						{
							availableNodes.RemoveAt(i - 1);
							availableNodes.Insert(i - 1, new UnaryNode(numberNode, x => x == 0 ? 1 : 0));
							break;
						}
						case "~":
						{
							availableNodes.RemoveAt(i - 1);
							availableNodes.Insert(i - 1, new UnaryNode(numberNode, x => ~(int)x));
							break;
						}
						default:
						{
							throw new NotImplementedException($"{signNode.Text} is a valid unary operator but is not yet implemented.");
						}
					}

					i--;
				}
				else throw new NotImplementedException($"{signNode.Text} is not a valid unary operator!");
			}
		}

		while (true)
		{
			var preOperatorNodes = from x in availableNodes where x is PreOperatorNode select x as PreOperatorNode;

			if (!preOperatorNodes.Any()) break;
			else
			{
				int tier = preOperatorNodes.Min(x => x.Tier);

				for (int i = 0; i < availableNodes.Count; i++)
				{
					if (availableNodes[i] is PreOperatorNode node && node.Tier == tier)
					{
						var binaryNode = OperatorNodeFactory.CreateBinaryNode(availableNodes[i - 1], node, availableNodes[i + 1], this);

						availableNodes.RemoveAt(i - 1);
						availableNodes.RemoveAt(i - 1);
						availableNodes.RemoveAt(i - 1);

						availableNodes.Insert(i - 1, binaryNode);

						i--;
					}
				}
			}	
		}

		if (availableNodes.Count > 1)
		{
			throw new Exception("Tree could not be made!");
		}
		else return availableNodes.First();
	}

	private List<INode> CreateParams(Span<Token> tokens)
	{
		List<INode> nodes = new();
		int next = 0;

		int level = 1;
		for (int i = 0; i < tokens.Length; i++)
		{
			switch (tokens[i].TokenType)
			{
				case TokenType.OpenParen:
				{
					level++;
					break;
				}
				case TokenType.CloseParen:
				{
					level--;
					break;
				}
				case TokenType.Comma:
				{
					if (level == 1)
					{
						nodes.Add(CreateTree(tokens[next..i]));
						next = i + 1;
					}

					break;
				}
			}
		}

		nodes.Add(CreateTree(tokens[next..tokens.Length]));
		
		return nodes;
	}

	private int FindClosing(int start, Span<Token> tokens)
	{
		int level = 0;
		for (int i = start; i < tokens.Length; i++)
		{
			switch (tokens[i].TokenType)
			{
				case TokenType.OpenParen:
				{
					level++;
					break;
				}
				case TokenType.CloseParen:
				{
					level--;

					if (level == 0)
					{
						if (_matches[tokens[start].Text[0]] == tokens[i].Text[0]) return i;
						else throw new Exception("Parentheses do not match!");
					}

					break;
				}
			}
		}

		throw new Exception($"Parentheses do not match! (level: {level})");
	}

	private readonly Dictionary<char, char> _matches = new() { ['('] = ')', ['['] = ']', ['{'] = '}' };

	public double GetVariable(string s)
	{
		if (!_variables.ContainsKey(s)) _variables.Add(s, 0);

		return _variables[s];
	}

	public double SetVariable(string s, double x)
	{
		if (_variables.ContainsKey(s)) _variables[s] = x;
		else _variables.Add(s, x);

		return x;
	}

	internal INode GetMacro(string s)
	{
		if (!_macros.ContainsKey(s)) _macros.Add(s, new ConstantNode(0));

		return _macros[s];
	}

	internal INode SetMacro(string s, INode x)
	{
		if (_macros.ContainsKey(s)) _macros[s] = x;
		else _macros.Add(s, x);

		return x;
	}

	internal Func<List<double>, double> GetFunction(string s) => _functions[s];
}
