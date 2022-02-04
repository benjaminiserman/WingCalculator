namespace WingCalculatorShared;
using System;
using System.Collections.Generic;
using System.Text;

internal static class Tokenizer
{
	private static readonly string _operatorCharacters = "~!%^&*-+=|<>/;:?";
	public static readonly string _unaryOperators = "+-$!@~";
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
				if (gotType == TokenType.Number && sb.ToString() == "0" && (c is 'b' or 'x'))
				{
					currentType = c is 'b' ? TokenType.Binary : TokenType.Hex;
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

		if (quoted) throw new Exception("Quote is missing end quote!");
		if (apostrophed) throw new Exception("Character is missing end quote!");

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
		TokenType.Function => char.IsLetter(c),
		TokenType.Hex => char.IsDigit(c) || _hexCharacters.Contains(c),
		TokenType.OpenParen => false,
		TokenType.CloseParen => false,
		TokenType.Comma => false,
		TokenType.Variable => char.IsLetter(c),
		TokenType.Macro => char.IsLetter(c),
		TokenType.Quote => false,
		TokenType.Char => false,
		TokenType.Binary => c is '1' or '0',

		_ => throw new NotImplementedException($"Unexpected character {c}! Current TokenType: {tokenType}")
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
		else throw new NotImplementedException($"Token could not be constructed from character {c}!");
	}
}

internal record Token(TokenType TokenType, string Text);

internal enum TokenType
{
	Number, Operator, Function, Hex, OpenParen, CloseParen, Comma, Variable, Macro, Quote, Char, Binary
}
