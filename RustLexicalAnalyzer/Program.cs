using System;
using System.IO;
using System.Text;
using RustLexicalAnalyzer.Analyzer;
using System.Collections.Generic;
using System.Linq;

namespace RustLexicalAnalyzer
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var inputFile = args.Length > 0 ? args[0] : "hello.rs";
			Console.OutputEncoding = Encoding.UTF8;
			Case1(inputFile);
			Case2(inputFile);
		}

		private static void Case1(string fileName)
		{
			
			var hello = File.Open(fileName, FileMode.Open);
			var lexicalAnalyzer = new LexicalAnalyzer(hello);
			var tokens = lexicalAnalyzer.GetTokens(200);
			Console.WriteLine(tokens.Length);
			hello.Close();
		}

		private static void Case2(string fileName)
		{
			var hello = File.Open(fileName, FileMode.Open);
			var lexicalAnalyzer = new LexicalAnalyzer(hello);
			while(!lexicalAnalyzer.IsEnded)
				Console.WriteLine(lexicalAnalyzer.GetNextToken());
			hello.Close();
		}
	}
}