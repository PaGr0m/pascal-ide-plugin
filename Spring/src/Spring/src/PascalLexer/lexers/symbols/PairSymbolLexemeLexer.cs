using System.Collections.Generic;
using System.Text;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.symbols
{
    public class PairSymbolLexemeLexer : ILexemeLexer
    {
        private readonly HashSet<string> _specialPairCharacters = new HashSet<string>()
        {
            "<<", ">>", "**", "><",
            "<>", "<=", ">=",
            ":=", "+=", "-=", "*=", "/=",
            "(.", ".)",
            "(*", "*)", "//"
        };

        public Lexeme Run(int currentPosition, ref string text)
        {
            var startPosition = currentPosition;
            var result = new StringBuilder();

            var symbol = text[currentPosition];
            currentPosition++;
            result.Append(symbol);

            if (currentPosition == text.Length) return null;

            symbol = text[currentPosition];
            currentPosition++;
            result.Append(symbol);

            if (!_specialPairCharacters.Contains(result.ToString())) return null;

            return new Lexeme(result.ToString(), LexemeKind.PairSpecialCharacter, startPosition, currentPosition);
        }
    }
}