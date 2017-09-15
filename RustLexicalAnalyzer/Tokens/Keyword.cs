using System.Collections.Generic;
using System.ComponentModel;

namespace RustLexicalAnalyzer.Tokens
{
    public class Keyword : SimpleToken
    {
        public override List<string> Type { get; } = new List<string>
        {
            "_",
            "as",
            // TODO add remaining Keywords.
        };
    }
}