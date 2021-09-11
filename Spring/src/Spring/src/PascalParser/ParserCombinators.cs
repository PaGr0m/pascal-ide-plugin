using System;
using System.Linq;
using JetBrains.ReSharper.Plugins.Spring.Spring;
using JetBrains.ReSharper.Psi.TreeBuilder;

namespace JetBrains.ReSharper.Plugins.Spring.PascalParser
{
    public static class ParserCombinators
    {
        public static bool Token(PsiBuilder builder, SpringTokenType expectedType)
        {
            if (builder.GetTokenType() != expectedType) return false;

            builder.AdvanceLexer();

            while (
                !builder.Eof() &&
                (builder.GetTokenType().IsWhitespace || builder.GetTokenType().IsComment)
            )
            {
                builder.AdvanceLexer();
            }

            return true;
        }

        public static bool Token(PsiBuilder builder, SpringTokenType expectedType, string expectedText)
        {
            if (
                builder.GetTokenType() != expectedType ||
                builder.GetTokenText() != expectedText
            ) return false;

            builder.AdvanceLexer();

            while (
                !builder.Eof() &&
                (builder.GetTokenType().IsWhitespace || builder.GetTokenType().IsComment)
            )
            {
                builder.AdvanceLexer();
            }

            return true;
        }

        public static Predicate<PsiBuilder> Partial<TT, TR>(Func<PsiBuilder, TT, TR, bool> func, TT t, TR r) =>
            builder => func(builder, t, r);

        public static Predicate<PsiBuilder> Partial<TT>(Func<PsiBuilder, TT, bool> func, TT t) =>
            builder => func(builder, t);

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
            foreach (var predicate in parsers)
            {
                if (predicate(builder))
                {
                    builder.Done(mark, SpringCompositeNodeType.BLOCK, null);
                    return true;
                }

                builder.RollbackTo(mark);
                mark = builder.Mark();
            }

            builder.Drop(mark);
            return false;
        };

        public static Predicate<PsiBuilder> Many(Predicate<PsiBuilder> parser) => builder =>
        {
            var mark = builder.Mark();
            var completeMark = builder.Mark();
            while (parser(builder))
            {
                builder.Drop(completeMark);
                completeMark = builder.Mark();
            }

            builder.RollbackTo(completeMark);
            builder.Done(mark, SpringCompositeNodeType.BLOCK, null);

            return true;
        };
    }
}