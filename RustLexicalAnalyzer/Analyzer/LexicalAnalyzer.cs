using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Text.RegularExpressions;
using static RustLexicalAnalyzer.Analyzer.Token.Types;

namespace RustLexicalAnalyzer.Analyzer
{
    public class LexicalAnalyzer
    {
        private int offset = 1;
        private int line = 1;
        private readonly StreamReader streamReader;
        private readonly List<char> buffer = new List<char>();

        public LexicalAnalyzer(Stream inputStream)
        {
            streamReader = new StreamReader(inputStream, Encoding.UTF8);
        }

        private char[] GetNext(int amount = 1)
        {
            var arr = new char[amount];
            for (var i = 0; i < buffer.Count && i < amount; i++)
                arr[i] = buffer[i];
            amount -= buffer.Count;
            buffer.Clear();
            
            if (streamReader.EndOfStream)
                throw new EndOfStreamException("The strean is ended");
            if (amount > 0)
                streamReader.Read(arr, 0, amount);
            return arr;
        }

        public Token[] GetNextTokens(int amount = 1)
        {
            if (streamReader.EndOfStream)
                return new Token[0];
            var startOffset = offset;
            var startLine = line;
            var tokens = new List<Token>();
            var sBuffer = "";
            if (buffer.Count > 0)
                startOffset--;

            // 0 - ident
            // 1 - none
            // 2 - keyword
            var flags = new bool[3];

            while (true)
            {
                var c = GetNext()[0];
                offset++;
                sBuffer += c;
                if (Regex.IsMatch(sBuffer, "^([^A-Za-z])") && Regex.IsMatch(sBuffer, "^([^_])"))
                    flags[1] = true;
                if (Regex.IsMatch(sBuffer, "^(?:[A-Za-z][A-Za-z0-9]*|_[A-Za-z0-9]+)$"))
                    flags[0] = true;
                else if (flags[0])
                {
                    buffer.Add(c);
                    sBuffer = sBuffer.Substring(0, sBuffer.Length - 1);
                    break;
                }
                if (flags[1] && Token.GetType(sBuffer) != NONE)
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

            tokens.Add(new Token((startLine, startOffset), type, sBuffer));
            return tokens.ToArray();
        }
    }
}