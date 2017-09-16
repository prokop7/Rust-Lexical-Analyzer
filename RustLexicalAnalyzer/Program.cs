using System;
using System.IO;
using System.Text;
using RustLexicalAnalyzer.Analyzer;
using static RustLexicalAnalyzer.Analyzer.Token.Types;
using System.Collections.Generic;
using System.Linq;

namespace RustLexicalAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Case2("hello.rs");
            Case("fn main() {}");
        }
          
		static void Case2(string fileName)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var t1 = new Token((1, 2), ANDAND, string.Empty);
            var t2 = new Token((1, 2), IDENT, "main");
            var hello = File.Open(fileName, FileMode.Open);
            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer(hello);
            for (int i = 0; i < 100; i++)
            {
                foreach (var token in lexicalAnalyzer.GetNextTokens())
                {
                    Console.WriteLine(token);
                }
        }

        static void Case(string code)
        {
            Console.WriteLine($"Code: `{code}`");
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(code));
            new Scanner(stream).Scan();
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    class Reader
    {
        private System.IO.Stream _stream;

        public Reader(System.IO.Stream stream)
        {
            _stream = stream;
        }

        public string Read(ICollection<string> allowed)
        {
            var source = new String(new System.IO.BinaryReader(_stream).ReadChars(Math.Min(allowed.Max(x => x.Length), (int)Left)));
            var matching = allowed.Where(x => source.StartsWith(x)).ToArray();
            if (matching.Length > 1)
            {
                throw new InvalidOperationException("multi match");
            }
            var result = matching.FirstOrDefault();
            Backtrack(source.Length - (result != null ? result.Length : 0));
            return result;
        }

        public string[] ReadRepeat(ICollection<string> allowed)
        {
            return Enumerable.Range(0, int.MaxValue)
                .Select(x => Read(allowed))
                .TakeWhile(x => x != null)
                .ToArray();
        }

        public string ReadIdent()
        {
            var reader = new System.IO.BinaryReader(_stream);
            var _0 = reader.PeekChar();
            if (_0 == -1 || !((char)_0).XID_Start())
            {
                return null;
            }
            var first = reader.ReadChar();
            var next = "";
            while (Left > 0) {
                if (!((char)reader.PeekChar()).XID_Continue()) {
                    break;
                }
                next += reader.ReadChar();
            }
            return first + next;
        }

        public long Left { get { return _stream.Length - _stream.Position; } }

        private void Backtrack(int length) {
            _stream.Seek(-length, System.IO.SeekOrigin.Current);
        }
    }

    static class Ext
    {
        public static bool XID_Start(this char x)
        {
            return "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(x);
        }

        public static bool XID_Continue(this char x)
        {
            return x.XID_Start() || "0123456789".Contains(x);
        }
    }

    class Scanner
    {
        public static readonly string[] Keywords = {
            "_",        "abstract", "alignof",  "as",       "become",
            "box",      "break",    "const",    "continue", "crate",
            "do",       "else",     "enum",     "extern",   "false",
            "final",    "fn",       "for",      "if",       "impl",
            "in",       "let",      "loop",     "macro",    "match",
            "mod",      "move",     "mut",      "offsetof", "override",
            "priv",     "proc",     "pub",      "pure",     "ref",
            "return",   "Self",     "self",     "sizeof",   "static",
            "struct",   "super",    "trait",    "true",     "type",
            "typeof",   "unsafe",   "unsized",  "use",      "virtual",
            "where",    "while",    "yield",
        };

        public static readonly string[] Whitespace = {
            "\x20", "\t", "\r", "\n",
        };

        public static readonly string[] Symbol = {
            "::", "->", "#", "[", "]", "(", ")", "{", "}", ",", ";",
        };

        public Scanner(System.IO.Stream stream)
        {
            _reader = new Reader(stream);
        }

        public void Scan()
        {
            var _left = -1L;
            while (_reader.Left > 0)
            {
                if (_left == _reader.Left)
                {
                    throw new InvalidOperationException("infinite loop");
                }
                _left = _reader.Left;
                Pass();
            }
        }

        private Reader _reader;

        private void Pass()
        {
            var _0 = _reader.Read(Keywords);
            if (_0 != null)
            {
                Console.WriteLine($"keyword({_0}) ");
                return;
            }

            var _1 = _reader.Read(Symbol);
            if (_1 != null)
            {
                Console.WriteLine($"symbol({_1}) ");
                return;
            }

            var _2 = _reader.ReadRepeat(Whitespace);
            if (_2.Length != 0)
            {
                Console.WriteLine($"whitespace({_2.Length}) ");
                return;
            }

            var _3 = _reader.ReadIdent();
            if (_3 != null)
            {
                Console.WriteLine($"ident({_3}) ");
                return;
            }
        }
    }
}