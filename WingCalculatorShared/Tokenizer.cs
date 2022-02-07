namespace WingCalculatorShared;
using System.Collections.Generic;
using System.Text;
using WingCalculatorShared.Exceptions;

internal static class Tokenizer
{
	private static readonly string _operatorCharacters = "~!%^&*-+=|<>/;:?";
	public static readonly string _unaryOperators = "+-$!@~`";
	private static readonly string _hexCharacters = "ABCDEFabcdef";
	private static readonly string _openParenCharacters = "([{";
	private static readonly string _closeParenCharacters = ")]}";

	public static List<Token> Tokenize(string s)
	{
		List<Token> tokens = new();

		bool apostrophed = false;
		bool quoted = false;
		TokenType? currentType = null;
		StringBuilder sb = new();
		foreach (char c in s)
		{
			if (c == '\'' && !quoted)
			{
				if (apostrophed)
				{
					PushCurrent();
				}
				else
				{
					PushCurrent();
					currentType = TokenType.Char;
				}

				apostrophed = !apostrophed;
			}
			else if (c == '\"' && !apostrophed)
			{
				if (quoted)
				{
					PushCurrent();
				}
				else
				{
					PushCurrent();
					currentType = TokenType.Quote;
				}

				quoted = !quoted;

			}
			else if (quoted || apostrophed) sb.Append(c);
			else if (c == '_') continue;
			else if (char.IsWhiteSpace(c))
			{
				PushCurrent();
			}
			else if (c == ',')
			{
				PushCurrent();
				Push(TokenType.Comma, c.ToString());
			}
			else if (_openParenCharacters.Contains(c))
			{
				PushCurrent();
				Push(TokenType.OpenParen, c.ToString());
			}
			else if (_closeParenCharacters.Contains(c))
			{
				PushCurrent();
				Push(TokenType.CloseParen, c.ToString());
			}
			else if (currentType is not TokenType gotType)
			{
				sb.Append(c);
				currentType = GetTokenType(c);
			}
			else
			{
				if (gotType == TokenType.Number && sb.ToString() == "0" && char.IsLetter(c))
				{
					currentType = char.ToLowerInvariant(c) switch
					{
						'b' => TokenType.Binary,
						'x' => TokenType.Hex,
						'o' => TokenType.Octal,

						_ => throw new WingCalcException($"0{c} is not a recognized numeric literal.")
					};
				}
				else if (Matches(gotType, c, sb))
				{
					sb.Append(c);
				}
				else
				{
					PushCurrent();

					sb.Append(c);
					currentType = GetTokenType(c);
				}
			}
		}

		PushCurrent();

		if (quoted) throw new WingCalcException($"Quote literal \"{tokens[^1].Text} is missing end quote.");
		if (apostrophed) throw new WingCalcException($"Character literal \'{tokens[^1].Text} is missing end apostrophe.");

		int additiveUnaryCount = 0;
		for (int i = 0; i < tokens.Count; i++)
		{
			if (tokens[i].TokenType == TokenType.Operator && tokens[i].Text is "+" or "-")
			{
				additiveUnaryCount++;
				if (additiveUnaryCount > 2) // if 3 +/- are found in a row
				{
					throw new WingCalcException($"Unexpected character '{tokens[i].Text}' found. Only up to two + or - signs in a row are legal.");
				}
				else if (additiveUnaryCount == 2 && i == 1) // if 2 +/- at start
				{
					throw new WingCalcException($"Unexpected character '{tokens[i].Text}' found. Only one + or - sign is legal at the start of an equation.");
				}
				else if (additiveUnaryCount == 2 && i == tokens.Count - 1) // if 2 +/- at end
				{
					throw new WingCalcException($"Unexpected character '{tokens[i].Text}' found. Only one + or - sign is legal at the end of an equation.");
				}
			}
			else additiveUnaryCount = 0;
		}

		for (int i = 0; i < tokens.Count - 2; i++)
		{
			if (tokens[i].Text == "**" && tokens[i + 1].Text == "-"  && tokens[i + 2].TokenType != TokenType.OpenParen)
			{
				tokens.Insert(i + 1, new Token(TokenType.OpenParen, "("));
				tokens.Insert(i + 4, new Token(TokenType.CloseParen, ")"));
			}
		}

		return tokens;

		void PushCurrent()
		{
			if (currentType is TokenType gotType)
			{
				Push(gotType, sb.ToString());
				currentType = null;
				sb.Clear();
			}
		}

		void Push(TokenType currentType, string s)
		{
			tokens.Add(new(currentType, s));
		}
	}

	private static bool Matches(TokenType tokenType, char c, StringBuilder sb) => tokenType switch
	{
		TokenType.Number when char.IsDigit(c) || ".Ee".Contains(c) => true,
		TokenType.Number when "+-".Contains(c) => "Ee".Contains(sb[^1]),
		TokenType.Number => false,
		TokenType.Operator => _operatorCharacters.Contains(c) && !_unaryOperators.Contains(c),
		TokenType.OpenParen => false,
		TokenType.CloseParen => false,
		TokenType.Comma => false,
		TokenType.Quote => false,
		TokenType.Char => false,
		TokenType.Local => false,

		_ when c is '.' => throw new WingCalcException($"Unexpected character '{c}' found!"),

		TokenType.Function => char.IsLetter(c),
		TokenType.Hex => char.IsDigit(c) || _hexCharacters.Contains(c),
		TokenType.Variable => char.IsLetter(c),
		TokenType.Macro => char.IsLetter(c),
		TokenType.Binary => c is '1' or '0',
		TokenType.Octal => char.IsDigit(c) && (c - '0') < 8,

		_ => false
	};

	private static TokenType GetTokenType(char c)
	{
		if (char.IsDigit(c) || ".".Contains(c)) return TokenType.Number;
		else if (_operatorCharacters.Contains(c)) return TokenType.Operator;
		else if (char.IsLetter(c)) return TokenType.Function;
		else if (c == '#') return TokenType.Hex;
		else if (_openParenCharacters.Contains(c)) return TokenType.OpenParen;
		else if (_closeParenCharacters.Contains(c)) return TokenType.CloseParen;
		else if (c == ',') return TokenType.Comma;
		else if (c == '$') return TokenType.Variable;
		else if (c == '@') return TokenType.Macro;
		else if (c == '`') return TokenType.Local
		else throw new WingCalcException($"Token could not be constructed from character {c}.");
	}
}

internal record Token(TokenType TokenType, string Text);

internal enum TokenType
{
	Number, Operator, Function, Hex, OpenParen, CloseParen, Comma, Variable, Macro, Quote, Char, Binary, Octal, Local
}
