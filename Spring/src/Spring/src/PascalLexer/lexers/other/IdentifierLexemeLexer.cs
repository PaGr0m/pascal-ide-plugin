using System.Collections.Generic;
using System.Text;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.other
{
    public class IdentifierLexemeLexer : ILexemeLexer
    {
        public Lexeme Run(int currentPosition, ref string text)
        {
            var startPosition = currentPosition;
            var value = new StringBuilder();
            var symbol = text[currentPosition];

            if (symbol.Equals('&'))
            {
                value.Append(symbol);
                currentPosition++;
                symbol = text[currentPosition];
            }

            if (!char.IsLetter(symbol) && !symbol.Equals('_')) return null;
            value.Append(symbol);
            currentPosition++;

            while (currentPosition < text.Length)
            {
                symbol = text[currentPosition];
                if (!char.IsLetterOrDigit(symbol)) break;
                value.Append(symbol);
                currentPosition++;
            }

            return Keywords.Contains(value.ToString())
                ? new Lexeme(value.ToString(), LexemeKind.Keyword, startPosition, currentPosition)
                : new Lexeme(value.ToString(), LexemeKind.Identifier, startPosition, currentPosition);
        }

        private static readonly List<string> Keywords = new List<string>()
        {
            "as", "in", "is",

            "not", "or", "xor",

            "div", "mod", "and", "shl", "shr", "as",

            "include", "exclude",

            "begin", "end",

            "case", "of", "otherwise",
            "if", "then", "else",

            "for", "to", "downto", "do",
            "while",
            "repeat", "until",

            "with", "goto"
        };
    }
}