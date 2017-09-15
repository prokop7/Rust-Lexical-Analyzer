using System;
using static RustLexicalAnalyzer.Token.Types;

namespace RustLexicalAnalyzer
{

    class Program
    {
        private static void Main(string[] args)
        {
            var t1 = new Token((1, 2), ANDAND, string.Empty);
            var t2 = new Token((1, 2), IDENT, "main");
            Console.WriteLine(t1);
            Console.WriteLine(t2);
        }
    }
}