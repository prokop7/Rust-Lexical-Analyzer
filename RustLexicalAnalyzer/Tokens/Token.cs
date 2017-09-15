namespace RustLexicalAnalyzer.Tokens
{
    public class Token
    {
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
    }
}