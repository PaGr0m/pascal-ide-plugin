using System.Text;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.comments
{
    public class SingleCommentLexemeLexer : ILexemeLexer
    {
        public Lexeme Run(int currentPosition, ref string text)
        {
            var startPosition = currentPosition;
            var isGoodComment = false;

            var value = new StringBuilder();
            var symbol = text[currentPosition];

            if (symbol.Equals('/'))
            {
                value.Append(symbol);
                currentPosition++;

                if (symbol.Equals('/'))
                {
                    value.Append(symbol);
                    currentPosition++;

                    while (currentPosition < text.Length)
                    {
                        symbol = text[currentPosition];
                        value.Append(symbol);
                        currentPosition++;

                        if (symbol.Equals('\n') || symbol.Equals('\r') || currentPosition == text.Length)
                        {
                            isGoodComment = true;
                            break;
                        }
                    }
                }
            }

            if (!isGoodComment) return null;

            return new Lexeme(value.ToString(), LexemeKind.SingleComment, startPosition, currentPosition);
        }
    }
}