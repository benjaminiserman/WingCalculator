namespace WingCalculatorShared;
using System;
using System.Collections.Generic;
using System.Linq;

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
		["sqrt"] = args => Math.Sqrt(args[0]),
		["cbrt"] = args => Math.Cbrt(args[0]),
		["ceil"] = args => Math.Ceiling(args[0]),
		["floor"] = args => Math.Floor(args[0]),
		["sin"] = args => Math.Sin(args[0]),
		["cos"] = args => Math.Cos(args[0]),
		["tan"] = args => Math.Tan(args[0]),
		["arcsin"] = args => Math.Asin(args[0]),
		["arccos"] = args => Math.Acos(args[0]),
		["arctan"] = args => Math.Atan(args[0]),
		["abs"] = args => Math.Abs(args[0]),
		["log"] = args => Math.Log(args[0], args[1]),
		["ln"] = args => Math.Log(args[0]),
	};

	private readonly Dictionary<string, INode> _macros = new();

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
					availableNodes.Add(new QuoteNode(tokens[i].Text));
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
						default:
						{
							throw new NotImplementedException($"{signNode.Text} is valid but not yet implemented.");
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
						var binaryNode = OperatorNodeFactory.CreateBinaryNode(availableNodes[i - 1], node, availableNodes[i + 1]);

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
			Console.WriteLine("Erroring:");
			foreach (var node in availableNodes) Console.WriteLine(node);
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

		foreach (Token token in tokens)
		{
			Console.WriteLine(token);
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
