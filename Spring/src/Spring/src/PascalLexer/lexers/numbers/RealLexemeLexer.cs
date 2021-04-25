using System.Text;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.numbers
{
    public class RealLexemeLexer : ILexemeLexer
    {
        private const string DigitPattern = "[0-9]";

        public Lexeme Run(int currentPosition, ref string text)
        {
            var startPosition = currentPosition;
            var result = new StringBuilder();

            var digitSequence = NumberLexerUtils.DigitSequence(currentPosition, ref text, DigitPattern);
            if (digitSequence == null) return null;
            result.Append(digitSequence);
            currentPosition += digitSequence.Length;
            if (currentPosition == text.Length) return null;

            var symbol = text[currentPosition];
            if (!symbol.Equals('.')) return null;

            result.Append(symbol);
            currentPosition++;
            if (currentPosition == text.Length)
            {
                return new Lexeme(result.ToString(), LexemeKind.Real, startPosition, currentPosition);
            }
            
            symbol = text[currentPosition];
            if (symbol.Equals('E') || symbol.Equals('e')) goto ScaleFactor;

            digitSequence = NumberLexerUtils.DigitSequence(currentPosition, ref text, DigitPattern);
            if (digitSequence == null)
            {
                return new Lexeme(result.ToString(), LexemeKind.Real, startPosition, currentPosition);
            }

            result.Append(digitSequence);
            currentPosition += digitSequence.Length;

            if (currentPosition == text.Length)
            {
                return new Lexeme(result.ToString(), LexemeKind.Real, startPosition, currentPosition);
            }

            symbol = text[currentPosition];

            if (!symbol.Equals('E') && !symbol.Equals('e'))
            {
                return new Lexeme(result.ToString(), LexemeKind.Real, startPosition, currentPosition);
            }
            
            ScaleFactor:
            result.Append(symbol);
            currentPosition++;

            symbol = text[currentPosition];
            if (symbol.Equals('+') || symbol.Equals('-'))
            {
                result.Append(symbol);
                currentPosition++;
            }

            digitSequence = NumberLexerUtils.DigitSequence(currentPosition, ref text, DigitPattern);
            result.Append(digitSequence);
            currentPosition += digitSequence.Length;

            return new Lexeme(result.ToString(), LexemeKind.Real, startPosition, currentPosition);
        }
    }
}