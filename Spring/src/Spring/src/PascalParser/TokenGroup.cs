using System;
using JetBrains.ReSharper.Plugins.Spring.Spring;
using JetBrains.ReSharper.Psi.TreeBuilder;
using static JetBrains.ReSharper.Plugins.Spring.PascalParser.ParserCombinators;

namespace JetBrains.ReSharper.Plugins.Spring.PascalParser
{
    public static class TokenGroup
    {
        public static readonly Predicate<PsiBuilder> Dot =
            Partial(Token, SpringTokenType.SingleSpecialCharacter, ".");

        public static readonly Predicate<PsiBuilder> Comma =
            Partial(Token, SpringTokenType.SingleSpecialCharacter, ",");

        public static readonly Predicate<PsiBuilder> LeftParenthesis =
            Partial(Token, SpringTokenType.SingleSpecialCharacter, "(");

        public static readonly Predicate<PsiBuilder> RightParenthesis =
            Partial(Token, SpringTokenType.SingleSpecialCharacter, ")");

        public static readonly Predicate<PsiBuilder> LeftSquareBracket =
            Partial(Token, SpringTokenType.SingleSpecialCharacter, "[");

        public static readonly Predicate<PsiBuilder> RightSquareBracket =
            Partial(Token, SpringTokenType.SingleSpecialCharacter, "]");

        public static readonly Predicate<PsiBuilder> Range =
            Partial(Token, SpringTokenType.PairSpecialCharacter, "..");

        public static readonly Predicate<PsiBuilder> Colon =
            Partial(Token, SpringTokenType.SingleSpecialCharacter, ":");

        public static readonly Predicate<PsiBuilder> Semicolon =
            Partial(Token, SpringTokenType.SingleSpecialCharacter, ";");

        public static readonly Predicate<PsiBuilder> KeywordAppropriation =
            Partial(Token, SpringTokenType.PairSpecialCharacter, ":=");

        public static readonly Predicate<PsiBuilder> AssignmentOperators = Alternative(
            Partial(Token, SpringTokenType.PairSpecialCharacter, ":="),
            Partial(Token, SpringTokenType.PairSpecialCharacter, "+="),
            Partial(Token, SpringTokenType.PairSpecialCharacter, "-="),
            Partial(Token, SpringTokenType.PairSpecialCharacter, "*="),
            Partial(Token, SpringTokenType.PairSpecialCharacter, "/=")
        );

        public static readonly Predicate<PsiBuilder> UnsignedConstant = Alternative(
            Partial(Token, SpringTokenType.Number),
            Partial(Token, SpringTokenType.StringLiteral),
            Partial(Token, SpringTokenType.Identifier),
            Partial(Token, SpringTokenType.Keyword, "Nil")
        );

        public static readonly Predicate<PsiBuilder> RelationalOperators = Alternative(
            Partial(Token, SpringTokenType.SingleSpecialCharacter, "<"),
            Partial(Token, SpringTokenType.SingleSpecialCharacter, ">"),
            Partial(Token, SpringTokenType.SingleSpecialCharacter, "="),
            Partial(Token, SpringTokenType.PairSpecialCharacter, "<="),
            Partial(Token, SpringTokenType.PairSpecialCharacter, ">="),
            Partial(Token, SpringTokenType.PairSpecialCharacter, "<>"),
            Partial(Token, SpringTokenType.Keyword, "in"),
            Partial(Token, SpringTokenType.Keyword, "is")
        );

        public static readonly Predicate<PsiBuilder> AddingOperators = Alternative(
            Partial(Token, SpringTokenType.SingleSpecialCharacter, "+"),
            Partial(Token, SpringTokenType.SingleSpecialCharacter, "-"),
            Partial(Token, SpringTokenType.Keyword, "or"),
            Partial(Token, SpringTokenType.Keyword, "xor")
        );

        public static readonly Predicate<PsiBuilder> MultiplicationOperators = Alternative(
            Partial(Token, SpringTokenType.SingleSpecialCharacter, "*"),
            Partial(Token, SpringTokenType.SingleSpecialCharacter, "/"),
            Partial(Token, SpringTokenType.Keyword, "div"),
            Partial(Token, SpringTokenType.Keyword, "mod"),
            Partial(Token, SpringTokenType.Keyword, "and"),
            Partial(Token, SpringTokenType.Keyword, "shl"),
            Partial(Token, SpringTokenType.Keyword, "shr"),
            Partial(Token, SpringTokenType.Keyword, "as"),
            Partial(Token, SpringTokenType.PairSpecialCharacter, "<<"),
            Partial(Token, SpringTokenType.PairSpecialCharacter, ">>")
        );

        public static readonly Predicate<PsiBuilder> LogicalOperators = Alternative(
            Partial(Token, SpringTokenType.PairSpecialCharacter, "**"),
            Partial(Token, SpringTokenType.Keyword, "not"),
            Partial(Token, SpringTokenType.Keyword, "and"),
            Partial(Token, SpringTokenType.Keyword, "or"),
            Partial(Token, SpringTokenType.Keyword, "xor"),
            Partial(Token, SpringTokenType.Keyword, "shl"),
            Partial(Token, SpringTokenType.Keyword, "shr")
        );

        public static readonly Predicate<PsiBuilder> BooleanOperators = Alternative(
            Partial(Token, SpringTokenType.Keyword, "not"),
            Partial(Token, SpringTokenType.Keyword, "and"),
            Partial(Token, SpringTokenType.Keyword, "or"),
            Partial(Token, SpringTokenType.Keyword, "xor")
        );

        public static readonly Predicate<PsiBuilder> StringsOperator =
            Partial(Token, SpringTokenType.SingleSpecialCharacter, "+");

        // Keywords
        public static readonly Predicate<PsiBuilder> KeywordGoto = Partial(Token, SpringTokenType.Keyword, "goto");
        public static readonly Predicate<PsiBuilder> KeywordBegin = Partial(Token, SpringTokenType.Keyword, "begin");
        public static readonly Predicate<PsiBuilder> KeywordEnd = Partial(Token, SpringTokenType.Keyword, "end");
        public static readonly Predicate<PsiBuilder> KeywordIf = Partial(Token, SpringTokenType.Keyword, "if");
        public static readonly Predicate<PsiBuilder> KeywordThen = Partial(Token, SpringTokenType.Keyword, "then");
        public static readonly Predicate<PsiBuilder> KeywordElse = Partial(Token, SpringTokenType.Keyword, "else");
        public static readonly Predicate<PsiBuilder> KeywordCase = Partial(Token, SpringTokenType.Keyword, "case");
        public static readonly Predicate<PsiBuilder> KeywordOf = Partial(Token, SpringTokenType.Keyword, "of");
        public static readonly Predicate<PsiBuilder> KeywordDo = Partial(Token, SpringTokenType.Keyword, "do");
        public static readonly Predicate<PsiBuilder> KeywordIn = Partial(Token, SpringTokenType.Keyword, "in");
        public static readonly Predicate<PsiBuilder> KeywordFor = Partial(Token, SpringTokenType.Keyword, "for");
        public static readonly Predicate<PsiBuilder> KeywordTo = Partial(Token, SpringTokenType.Keyword, "to");
        public static readonly Predicate<PsiBuilder> KeywordDownto = Partial(Token, SpringTokenType.Keyword, "downto");
        public static readonly Predicate<PsiBuilder> KeywordRepeat = Partial(Token, SpringTokenType.Keyword, "repeat");
        public static readonly Predicate<PsiBuilder> KeywordUntil = Partial(Token, SpringTokenType.Keyword, "until");
        public static readonly Predicate<PsiBuilder> KeywordWhile = Partial(Token, SpringTokenType.Keyword, "while");
        public static readonly Predicate<PsiBuilder> KeywordWith = Partial(Token, SpringTokenType.Keyword, "with");

        public static readonly Predicate<PsiBuilder> KeywordOtherwise =
            Partial(Token, SpringTokenType.Keyword, "otherwise");
    }
}