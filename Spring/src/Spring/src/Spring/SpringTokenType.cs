using System;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.Html;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;

namespace JetBrains.ReSharper.Plugins.Spring.Spring
{
    public class SpringTokenType : TokenNodeType
    {
        // TODO: change token && name
        public static readonly SpringTokenType Unknown = new SpringTokenType("Unknown", 0);
        
        public static readonly SpringTokenType Number = new SpringTokenType("Number", 1);
        public static readonly SpringTokenType Whitespace = new SpringTokenType("Whitespace", 2);
        public static readonly SpringTokenType Keyword = new SpringTokenType("Keyword", 3);
        public static readonly SpringTokenType Identifier = new SpringTokenType("Identifier", 5);
        public static readonly SpringTokenType Comment = new SpringTokenType("Comment", 4);

        public static readonly SpringTokenType SingleSpecialCharacter = new SpringTokenType("SSC", 8);


        // public static readonly SpringTokenType SingleSymbol = new SpringTokenType("SYMBOL", 3);
        // public static readonly SpringTokenType PairSymbol = new SpringTokenType("PAIR_SYMBOL", 3);
        public static readonly SpringTokenType StringLiteral = new SpringTokenType("STRING_LITERAL", 6);
        public static readonly SpringTokenType ConstantLiteral = new SpringTokenType("CONSTANT_LITERAL", 7);

        public SpringTokenType(string s, int index) : base(s, index)
        {
        }

        public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
        {
            return new LeafElement(
                new StringBuffer(
                    string.Concat(
                        buffer
                            .ToEnumerable()
                            .Skip(startOffset.Offset)
                            .Take(endOffset.Offset - startOffset.Offset)
                    )
                ),
                this
            );
        }

        public override bool IsKeyword => this == Keyword;
        public override bool IsWhitespace => this == Whitespace;


        public override bool IsStringLiteral => this == Number;
        public override bool IsIdentifier => this == Identifier;
        public override bool IsConstantLiteral => this == ConstantLiteral;
        public override bool IsComment => this == Comment;

        public override string TokenRepresentation => base.ToString();

        private class LeafElement : LeafElementBase, ITokenNode
        {
            private readonly IBuffer _buffer;
            private readonly SpringTokenType _springTokenType;

            public LeafElement(IBuffer buffer, SpringTokenType springTokenType)
            {
                _buffer = buffer;
                _springTokenType = springTokenType;
            }

            public override int GetTextLength()
            {
                return _buffer.Length;
            }

            public override StringBuilder GetText(StringBuilder to)
            {
                return to.Append(_buffer);
            }

            public override IBuffer GetTextAsBuffer()
            {
                return _buffer;
            }

            public override string GetText()
            {
                return _buffer.GetText();
            }

            public override NodeType NodeType => _springTokenType;

            public override PsiLanguageType Language => SpringLanguage.Instance;

            public TokenNodeType GetTokenType()
            {
                return _springTokenType;
            }
        }
    }
}