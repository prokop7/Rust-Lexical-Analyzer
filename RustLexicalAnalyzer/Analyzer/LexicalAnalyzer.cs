using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static RustLexicalAnalyzer.Analyzer.Token.Types;

namespace RustLexicalAnalyzer.Analyzer
{
	public class LexicalAnalyzer
	{
		/// <summary>
		/// Counting position in the input stream.
		/// </summary>
		private int offset = 1;

		private int line = 1;

		/// <summary>
		/// Internal Buffer for storing characters which are alreay read but not parsed.
		/// </summary>
		private readonly List<char> buffer = new List<char>();

		private readonly StreamReader streamReader;
		public bool IsEnded { get; private set; }

		public LexicalAnalyzer(Stream inputStream) => streamReader = new StreamReader(inputStream, Encoding.UTF8);

		/// <summary>
		/// <returns/> next <param name="amount"/> from input stream
		/// </summary>
		private char[] GetNext(int amount = 1)
		{
			var arr = new char[amount];
			for (var i = 0; i < buffer.Count && i < amount; i++)
				arr[i] = buffer[i];
			amount -= buffer.Count;
			buffer.Clear();
			if (amount == 0)
				return arr;
			if (streamReader.EndOfStream)
			{
				IsEnded = true;
				return new char[0];
			}
			if (amount > 0)
				streamReader.Read(arr, 0, amount);
			return arr;
		}

		/// <summary>
		/// <returns>Next <param name="amount"/> tokens from input stream</returns>
		/// </summary>
		public Token[] GetTokens(int amount)
		{
			var tokens = new List<Token>();
			for (var i = 0; i < amount; i++)
			{
				var token = GetNextToken();
				if (token != null)
					tokens.Add(token);
				else break;
			}
			return tokens.ToArray();
		}

		/// <summary>
		/// Parse sequence of the characters one by one.
		/// <returns>Only first next Token</returns>
		/// </summary>
		public Token GetNextToken()
		{
			if (streamReader.EndOfStream && buffer.Count == 0)
				return null;
			var startOffset = offset;
			var startLine = line;
			var sBuffer = "";
			if (buffer.Count > 0)
				offset--;

			// 0 - ident
			// 1 - none    // There is no reason to use it.
			// 2 - keyword
			var flags = new bool[3];
			flags[1] = true;

			while (true)
			{
				var charArr = GetNext();
				if (charArr.Length == 0)
					break;
				var c = charArr[0];
				offset++;
				sBuffer += c;
				
				//Regex matches identifiers
				if (Regex.IsMatch(sBuffer, "^(_[A-Za-z0-9_]+|[A-Za-z][A-Za-z0-9_]*)$"))
				{
					flags[0] = true;
					flags[1] = false;
				}
				else if (flags[0])
				{
					// If character doesn't belong to identifier, then it will be added to buffer
					// This character will be read in the next call
					buffer.Add(c);
					sBuffer = sBuffer.Substring(0, sBuffer.Length - 1);
					break;
				}
				if ((flags[1] || sBuffer == "_") && Token.GetType(sBuffer) != NONE)
					flags[2] = true;
				if (flags[2] && Token.GetType(sBuffer) == NONE)
				{
					buffer.Add(c);
					sBuffer = sBuffer.Substring(0, sBuffer.Length - 1);
					break;
				}
				if (flags[1] && !flags[2])
					break;
			}
			var type = Token.GetType(sBuffer);
			if (flags[0] && type == NONE)
				type = IDENT;
			if (type == NEW_LINE)
			{
				line++;
				offset = 1;
			}
			return new Token((startLine, startOffset), type, sBuffer);
		}
	}
}