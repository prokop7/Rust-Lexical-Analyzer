using System.Collections.Generic;

namespace RustLexicalAnalyzer.Tokens
{
    public class SimpleToken : Token
    {
        public virtual List<string> Type { get; }
    }
}