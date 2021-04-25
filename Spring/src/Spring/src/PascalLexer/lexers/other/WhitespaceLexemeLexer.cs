using System.Text;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.other
{
    public class WhitespaceLexemeLexer : ILexemeLexer
    {
        public Lexeme Run(int currentPosition, ref string text)
        {
            var value = new StringBuilder();
            var startPosition = currentPosition;

            if (!char.IsWhiteSpace(text[currentPosition]))
            {
                return null;
            }

            while (currentPosition < text.Length && char.IsWhiteSpace(text[currentPosition]))
            {
                value.Append(text[currentPosition]);
                currentPosition++;
            }

            return new Lexeme(value.ToString(), LexemeKind.Whitespace, startPosition, currentPosition);
        }
    }
}