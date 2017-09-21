using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace RustLexicalAnalyzer.Analyzer
{
	/// <summary>
	/// Extender for DisplayName property.
	/// </summary>
	public static class EnumExtensions
	{
		public static string GetDisplayName(this Enum enumValue)
		{
			return enumValue.GetType()
				.GetMember(enumValue.ToString())
				.First()
				.GetCustomAttribute<DisplayNameAttribute>()
				.DisplayName;
		}
	}
	
	public class Token
	{
		/// <summary>
		/// String token for Identifiers.
		/// </summary>
		public string StringToken { get; }
		
		
		/// <summary>
		/// Position of the token.
		/// </summary>
		public RefToSource ReferRefToSource { get; }
		
		public struct RefToSource
		{
			public readonly int LineNumb;
			public readonly int PosNumb;

			public static implicit operator (int, int)(RefToSource RefToSource) 
				=> (RefToSource.LineNumb, RefToSource.PosNumb);

			public static implicit operator RefToSource((int LineNumb, int PosNumb) RefToSource) 
				=> new RefToSource(RefToSource.LineNumb, RefToSource.PosNumb);

			public RefToSource(int lineNumb, int posNumb)
			{
				PosNumb = posNumb;
				LineNumb = lineNumb;
			}
		}

		/// <summary>
		/// Convert a string to Type of token.
		/// </summary>
		/// <param name="s">input string</param>
		/// <returns>Type of token which satisfied to string.</returns>
		public static Types GetType(string s)
		{
			foreach (Types type in Enum.GetValues(typeof(Types)))
			{
				if (s.Equals(type.GetDisplayName()) && !s.Equals(""))
					return type;
			}
			return Types.NONE;
		}
		
		/// <summary>
		/// All types of tokens.
		/// If none of them was matched then it's NONE.
		/// </summary>
		public enum Types
		{
			[DisplayName("")] IDENT,
			[DisplayName("")] NONE,
			[DisplayName("")] LITERAL,

			// Keywords
			[DisplayName("_")] _,
			[DisplayName("abstract")] ABSTRACT,
			[DisplayName("alignof")] ALIGNOF,
			[DisplayName("as")] AS,
			[DisplayName("become")] BECOME,
			[DisplayName("box")] BOX,
			[DisplayName("break")] BREAK,
			[DisplayName("const")] CONST,
			[DisplayName("continue")] CONTINUE,
			[DisplayName("crate")] CRATE,
			[DisplayName("do")] DO,
			[DisplayName("else")] ELSE,
			[DisplayName("enum")] ENUM,
			[DisplayName("extern")] EXTERN,
			[DisplayName("false")] FALSE,
			[DisplayName("final")] FINAL,
			[DisplayName("fn")] FN,
			[DisplayName("for")] FOR,
			[DisplayName("if")] IF,
			[DisplayName("impl")] IMPL,
			[DisplayName("in")] IN,
			[DisplayName("let")] LET,
			[DisplayName("loop")] LOOP,
			[DisplayName("macro")] MACRO,
			[DisplayName("match")] MATCH,
			[DisplayName("mod")] MOD,
			[DisplayName("move")] MOVE,
			[DisplayName("mut")] MUT,
			[DisplayName("offsetof")] OFFSETOF,
			[DisplayName("override")] OVERRIDE,
			[DisplayName("priv")] PRIV,
			[DisplayName("proc")] PROC,
			[DisplayName("pub")] PUB,
			[DisplayName("pure")] PURE,
			[DisplayName("ref")] REF,
			[DisplayName("return")] RETURN,
			[DisplayName("Self")] Self,
			[DisplayName("self")] SELF,
			[DisplayName("sizeof")] SIZEOF,
			[DisplayName("static")] STATIC,
			[DisplayName("struct")] STRUCT,
			[DisplayName("super")] SUPER,
			[DisplayName("trait")] TRAIT,
			[DisplayName("true")] TRAU,
			[DisplayName("type")] TYPE,
			[DisplayName("typeof")] TYPEOF,
			[DisplayName("unsafe")] UNSAFE,
			[DisplayName("unsized")] UNSIZED,
			[DisplayName("use")] USE,
			[DisplayName("virtual")] VIRTUAL,
			[DisplayName("where")] WHERE,
			[DisplayName("while")] WHILE,
			[DisplayName("yield")] YIELD,

			// Symbols
			[DisplayName("...")] DOTDOTDOT,
			[DisplayName("..")] DOTDOT,
			[DisplayName(".")] DOT,
			[DisplayName(",")] COMMA,
			[DisplayName(";")] SEMICOLON,
			[DisplayName("(")] OPEN_CURVE_BRACE,
			[DisplayName(")")] CLOSE_CURVE_BRACE,
			[DisplayName("[")] OPEN_SQUARE_BRACE,
			[DisplayName("]")] CLOSE_SQUARE_BRACE,
			[DisplayName("{")] OPEN_CURLY_BRACE,
			[DisplayName("}")] CLOSE_CURLY_BRACE,
			[DisplayName("@")] AT,
			[DisplayName("#")] HASH,

			[DisplayName("~")] TILDE,
			[DisplayName("::")] DOUBLE_COLON,
			[DisplayName(":")] COLON,
			[DisplayName("$")] DOLLAR,
			[DisplayName("?")] QUEST_MARK,

			[DisplayName("==")] EQEQ,
			[DisplayName("=>")] RIGHT_FAT_ARROW,
			[DisplayName("=")] EQ,
			[DisplayName("!=")] NOTEQ,
			[DisplayName("!")] EXCLAMATION_MARK,
			[DisplayName("<=")] LEQ,
			[DisplayName(">=")] GEQ,
			[DisplayName("<<")] SHL,
			[DisplayName("<<=")] SHLEQ,
			[DisplayName("<")] LESS,
			[DisplayName(">>")] SHR,
			[DisplayName(">>=")] SHREQ,
			[DisplayName(">")] GREATER,

			[DisplayName(" ")] SPACE,
			[DisplayName("\n")] NEW_LINE,
			[DisplayName("\r")] CARRET_RETURN,
			[DisplayName("\t")] TAB,

			[DisplayName("<-")] LEFT_ARROW,
			[DisplayName("->")] RIGHT_ARROW,
			[DisplayName("-")] MINUS,
			[DisplayName("-=")] MINUSEQ,
			[DisplayName("&&")] AMPAMP,
			[DisplayName("&")] AMP,
			[DisplayName("&=")] AMPEQ,
			[DisplayName("||")] OROR,
			[DisplayName("|")] OR,
			[DisplayName("|=")] OREQ,
			[DisplayName("+")] PLUS,
			[DisplayName("+=")] PLUSEQ,
			[DisplayName("*")] ASTER,
			[DisplayName("*=")] ASTEREQ,
			[DisplayName("/")] SLASH,
			[DisplayName("/=")] SLASHEQ,
			[DisplayName("^")] CARET,
			[DisplayName("^=")] CARETEQ,
			[DisplayName("%")] PERCENT,
			[DisplayName("%=")] PERCENTEQ,
		}

		public Types Type { get; }

		public Token(RefToSource referRefToSource, Types type, string stringToken = "")
		{
			ReferRefToSource = referRefToSource;
			Type = type;
			StringToken = type == Types.IDENT || type == Types.NONE ? stringToken : type.ToString();
		}


		public override string ToString()
		{
			var pos = $"[{ReferRefToSource.LineNumb,2}:{ReferRefToSource.PosNumb,-2}]";
			var res = $"{Type,-20}{pos}";
			if (Type == Types.NONE || Type == Types.IDENT)
				res += $"\tString: {StringToken}";
			return res;
		}
	}
}