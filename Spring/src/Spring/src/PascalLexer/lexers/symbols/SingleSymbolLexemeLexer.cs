using System.Collections.Generic;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.symbols
{
    public class SingleSymbolLexemeLexer : ILexemeLexer
    {
        private readonly HashSet<char> _specialCharacters = new HashSet<char>()
        {
            '+', '-', '*', '/',
            '^', '=',
            '<', '>', '[', ']', '(', ')', '{', '}',
            '.', ',', '’', ':', ';',
            '@', '$', '#', '&', '%'
        };

        public Lexeme Run(int currentPosition, ref string text)
        {
            var startPosition = currentPosition;

            var symbol = text[currentPosition];
            currentPosition++;

            if (!_specialCharacters.Contains(symbol)) return null;

            return new Lexeme(symbol.ToString(), LexemeKind.SingleSpecialCharacter, startPosition, currentPosition);
        }
    }
}