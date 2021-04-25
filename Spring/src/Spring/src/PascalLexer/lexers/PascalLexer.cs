using System.Collections.Generic;
using System.IO;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.comments;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.numbers;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.other;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.strings;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers.symbols;
using JetBrains.ReSharper.Plugins.Spring.Spring;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers
{
    public class PascalLexer : ILexer
    {
        private static readonly List<ILexemeLexer> Lexers = new List<ILexemeLexer>()
        {
            new WhitespaceLexemeLexer(),

            new IdentifierLexemeLexer(),

            new RealLexemeLexer(),
            new IntegerLexemeLexer(),

            new MultipleCommentLexemeLexer(),
            new SingleCommentLexemeLexer(),

            new ControlStringLexemeLexer(),
            new QuotedStringLexemeLexer(),

            new PairSymbolLexemeLexer(),
            new SingleSymbolLexemeLexer(),
        };

        private int _currentPosition;
        private string _text;
        private Lexeme _lexeme;

        public PascalLexer(IBuffer buffer)
        {
            Buffer = buffer;
            Start();
        }

        public void Start()
        {
            _currentPosition = 0;
            _text = Buffer.GetText();
        }

        public void Advance()
        {
            if (_currentPosition >= _text.Length)
            {
                _lexeme = null;
                return;
            }

            foreach (var lexer in Lexers)
            {
                _lexeme = lexer.Run(_currentPosition, ref _text);
                if (_lexeme == null) continue;

                _currentPosition = _lexeme.FinishPosition;
                break;
            }
        }

        public object CurrentPosition
        {
            get => _currentPosition;
            set => _currentPosition = (int) value;
        }

        public int TokenStart => _lexeme.StartPosition;

        public int TokenEnd => _lexeme.FinishPosition;

        public TokenNodeType TokenType
        {
            get
            {
                if (_lexeme == null) Advance();
                if (_lexeme == null) return null;

                return _lexeme.Kind switch
                {
                    LexemeKind.Whitespace => SpringTokenType.Whitespace,

                    LexemeKind.Keyword => SpringTokenType.Keyword,
                    LexemeKind.Identifier => SpringTokenType.Identifier,

                    LexemeKind.BinaryNumber => SpringTokenType.Number,
                    LexemeKind.OctalNumber => SpringTokenType.Number,
                    LexemeKind.HexNumber => SpringTokenType.Number,
                    LexemeKind.Integer => SpringTokenType.Number,
                    LexemeKind.Real => SpringTokenType.Number,

                    LexemeKind.SingleComment => SpringTokenType.Comment,
                    LexemeKind.MultipleComment => SpringTokenType.Comment,

                    LexemeKind.SingleSpecialCharacter => SpringTokenType.SingleSpecialCharacter,
                    LexemeKind.PairSpecialCharacter => SpringTokenType.PairSpecialCharacter,

                    LexemeKind.ControlString => SpringTokenType.StringLiteral,
                    LexemeKind.QuotedString => SpringTokenType.StringLiteral,

                    _ => SpringTokenType.Unknown
                };
            }
        }

        public IBuffer Buffer { get; }
    }
}