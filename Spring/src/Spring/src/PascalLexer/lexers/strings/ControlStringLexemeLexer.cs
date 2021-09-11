using System.Text;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.numbers;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.strings
{
    public class ControlStringLexemeLexer : ILexemeLexer
    {
        public Lexeme Run(int currentPosition, ref string text)
        {
            var startPosition = currentPosition;
            var result = new StringBuilder();
            var symbol = text[currentPosition];

            if (!symbol.Equals('#')) return null;

            result.Append(symbol);
            currentPosition++;

            var integerLexer = new IntegerLexemeLexer();
            var lexeme = integerLexer.Run(currentPosition, ref text);
            
            if (lexeme == null) return null;
            currentPosition = lexeme.FinishPosition;
            result.Append(lexeme.Text);

            return new Lexeme(result.ToString(), LexemeKind.ControlString, startPosition, currentPosition);
        }
    }
}