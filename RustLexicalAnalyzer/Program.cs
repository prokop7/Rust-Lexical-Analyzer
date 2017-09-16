using System;
using System.IO;
using System.Text;
using RustLexicalAnalyzer.Analyzer;
using static RustLexicalAnalyzer.Analyzer.Token.Types;

namespace RustLexicalAnalyzer
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var t1 = new Token((1, 2), ANDAND, string.Empty);
            var t2 = new Token((1, 2), IDENT, "main");
            var hello = File.Open("hello.rs", FileMode.Open);
            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer(hello);
//			Console.WriteLine(lexicalAnalyzer.GetNext(20));
            for (int i = 0; i < 100; i++)
            {
                foreach (var token in lexicalAnalyzer.GetNextTokens())
                {
                    Console.WriteLine(token);
                }
            }
        }
    }
}