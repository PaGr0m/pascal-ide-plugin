using System.Text;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.strings
{
    public class QuotedStringLexemeLexer : ILexemeLexer
    {
        public Lexeme Run(int currentPosition, ref string text)
        {
            var startPosition = currentPosition;
            var result = new StringBuilder();
            var symbol = text[currentPosition];

            if (symbol.Equals('\''))
            {
                result.Append(symbol);
                currentPosition++;

                StringCharacter:
                while (currentPosition < text.Length)
                {
                    symbol = text[currentPosition];
                    result.Append(symbol);
                    currentPosition++;

                    if (symbol.Equals('\''))
                    {
                        if (currentPosition < text.Length && text[currentPosition].Equals('\''))
                        {
                            result.Append(text[currentPosition]);
                            currentPosition++;

                            goto StringCharacter;
                        }

                        return new Lexeme(result.ToString(), LexemeKind.QuotedString, startPosition, currentPosition);
                    }
                }
            }

            return null;
        }
    }
}