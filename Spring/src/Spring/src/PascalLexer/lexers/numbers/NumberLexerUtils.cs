using System.Text;
using System.Text.RegularExpressions;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.numbers
{
    public static class NumberLexerUtils
    {
        public static string DigitSequence(int currentPosition, ref string text, string pattern)
        {
            var result = new StringBuilder();
            while (currentPosition < text.Length)
            {
                var symbol = text[currentPosition];
                if (!Regex.IsMatch(symbol.ToString(), pattern)) break;
                result.Append(symbol);
                currentPosition++;
            }

            return result.Length == 0 ? null : result.ToString();
        }
    }
}