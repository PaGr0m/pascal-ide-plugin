using System.Text;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.comments
{
    public class MultipleCommentLexemeLexer : ILexemeLexer
    {
        public Lexeme Run(int currentPosition, ref string text)
        {
            var startPosition = currentPosition;
            var isGoodComment = false;

            var value = new StringBuilder();
            var symbol = text[currentPosition];

            switch (symbol)
            {
                case '{':
                    while (currentPosition < text.Length)
                    {
                        symbol = text[currentPosition];
                        value.Append(symbol);
                        currentPosition++;

                        if (symbol.Equals('}'))
                        {
                            isGoodComment = true;
                            break;
                        }
                    }

                    break;

                case '(':
                    symbol = text[currentPosition];
                    value.Append(symbol);
                    currentPosition++;

                    if (!text[currentPosition].Equals('*'))
                    {
                        return null;
                    }

                    symbol = text[currentPosition];
                    value.Append(symbol);
                    currentPosition++;

                    while (currentPosition < text.Length)
                    {
                        symbol = text[currentPosition];
                        value.Append(symbol);
                        currentPosition++;

                        if (symbol.Equals('*') &&
                            currentPosition < text.Length &&
                            text[currentPosition].Equals(')'))
                        {
                            value.Append(text[currentPosition]);
                            currentPosition++;
                            isGoodComment = true;
                            break;
                        }
                    }

                    break;
            }

            if (!isGoodComment) return null;

            return new Lexeme(value.ToString(), LexemeKind.MultipleComment, startPosition, currentPosition);
        }
    }
}