using System.Text;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.numbers
{
    public class IntegerLexemeLexer : ILexemeLexer
    {
        private const string HexPattern = "[0-9A-Fa-f]";
        private const string DigitPattern = "[0-9]";
        private const string OctalPattern = "[0-7]";
        private const string BinaryPattern = "[0-1]";

        public Lexeme Run(int currentPosition, ref string text)
        {
            var startPosition = currentPosition;
            var result = new StringBuilder();
            var symbol = text[currentPosition];
            LexemeKind kind;

            string digitSequence;
            switch (symbol)
            {
                // Hex digit
                case '$':
                    result.Append(symbol);
                    currentPosition++;
                    digitSequence = NumberLexerUtils.DigitSequence(currentPosition, ref text, HexPattern);
                    if (digitSequence == null) return null;
                    result.Append(digitSequence);
                    currentPosition += digitSequence.Length;
                    kind = LexemeKind.HexNumber;
                    break;

                // Octal digit
                case '&':
                    result.Append(symbol);
                    currentPosition++;
                    digitSequence = NumberLexerUtils.DigitSequence(currentPosition, ref text, OctalPattern);
                    if (digitSequence == null) return null;
                    result.Append(digitSequence);
                    currentPosition += digitSequence.Length;
                    kind = LexemeKind.OctalNumber;
                    break;

                // Binary digit
                case '%':
                    result.Append(symbol);
                    currentPosition++;
                    digitSequence = NumberLexerUtils.DigitSequence(currentPosition, ref text, BinaryPattern);
                    if (digitSequence == null) return null;
                    result.Append(digitSequence);
                    currentPosition += digitSequence.Length;
                    kind = LexemeKind.BinaryNumber;
                    break;

                // Digit
                default:
                    digitSequence = NumberLexerUtils.DigitSequence(currentPosition, ref text, DigitPattern);
                    if (digitSequence == null) return null;
                    result.Append(digitSequence);
                    currentPosition += digitSequence.Length;
                    kind = LexemeKind.Integer;
                    break;
            }

            return new Lexeme(result.ToString(), kind, startPosition, currentPosition);
        }
    }
}