﻿using System;
using System.ComponentModel;

public class Token
{
	public RefToSource ReferRefToSource { get; }
	public string StringToken { get; }
	
	public enum Types
	{
		[DisplayName("")] IDENT,
		
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
		[DisplayName("!")] EXCLAMATIN_MARK,
		[DisplayName("<=")] LEQ,
		[DisplayName(">=")] GEQ,
		[DisplayName("<<")] SHL,
		[DisplayName("<<=")] SHLEQ,
		[DisplayName("<")] LESS,
		[DisplayName(">>")] SHR,
		[DisplayName(">>=")] SHREQ,
		[DisplayName(">")] GREATER,

		[DisplayName(" ")] SPACE,
		[DisplayName("\\n")] NEW_LINE,
		[DisplayName("\\r")] CARRET_RETURN,
		[DisplayName("\\t")] TAB,

		[DisplayName("<-")] LEFT_ARROW,
		[DisplayName("->")] RIGHT_ARROW,
		[DisplayName("-")] MINUS,
		[DisplayName("-=")] MINUSEQ,
		[DisplayName("&&")] ANDAND,
		[DisplayName("&")] AND,
		[DisplayName("&=")] ANDEQ,
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

	public struct RefToSource
	{
		public int LineNumb;
		public int PosNumb;

		public RefToSource(int lineNumb, int posNumb)
		{
			PosNumb = posNumb;
			LineNumb = lineNumb;
		}
	}

	public virtual Types Type { get; }

	public Token(RefToSource referRefToSource, Types type, string stringToken="")
	{
		ReferRefToSource = referRefToSource;
		Type = type;
		StringToken = type == Types.IDENT ? stringToken : type.ToString();
	}
}