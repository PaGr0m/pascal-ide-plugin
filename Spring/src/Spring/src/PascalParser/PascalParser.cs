using System;
using System.Linq;
using JetBrains.ReSharper.Plugins.Spring.Spring;
using JetBrains.ReSharper.Psi.TreeBuilder;
using Sprache;

namespace JetBrains.ReSharper.Plugins.Spring.PascalParser
{
    public static class PascalParser
    {
        public static readonly Predicate<PsiBuilder> LogicalOperation = Alternative(
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
            Partial(Token, SpringTokenType.SingleSpecialCharacter, "+"),
            Partial(Token, SpringTokenType.SingleSpecialCharacter, "-")
        );


        public static Predicate<PsiBuilder> Many(Predicate<PsiBuilder> parser) => builder =>
        {
            var mark = builder.Mark();
            while (parser(builder))
            {
            }

            builder.Done(mark, SpringCompositeNodeType.BLOCK, null);

            return true;
        };

        public static Predicate<PsiBuilder> Sequence(params Predicate<PsiBuilder>[] parsers) => builder =>
        {
            var mark = builder.Mark();
            if (parsers.Any(predicate => !predicate(builder)))
            {
                builder.Drop(mark);
                return false;
            }

            builder.Done(mark, SpringCompositeNodeType.BLOCK, null);
            return true;
        };

        public static Predicate<PsiBuilder> Alternative(params Predicate<PsiBuilder>[] parsers) => builder =>
        {
            var mark = builder.Mark();
            if (parsers.Any(predicate => predicate(builder)))
            {
                builder.Done(mark, SpringCompositeNodeType.BLOCK, null);
                return true;
            }

            builder.Drop(mark);
            return false;
        };

        public static Predicate<PsiBuilder> Partial<TT>(Func<PsiBuilder, TT, bool> func, TT t) =>
            builder => func(builder, t);

        public static Predicate<PsiBuilder> Partial<TT, TR>(Func<PsiBuilder, TT, TR, bool> func, TT t, TR r) =>
            builder => func(builder, t, r);

        public static bool Token(PsiBuilder builder, SpringTokenType expectedType, string expectedText)
        {
            if (
                builder.GetTokenType() != expectedType ||
                builder.GetTokenText() != expectedText
            ) return false;

            builder.AdvanceLexer();
            return true;
        }

        public static bool Token(PsiBuilder builder, SpringTokenType expectedType)
        {
            if (builder.GetTokenType() != expectedType) return false;

            builder.AdvanceLexer();
            return true;
        }
    }
}