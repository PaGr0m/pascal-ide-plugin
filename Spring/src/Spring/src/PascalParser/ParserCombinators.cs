using System;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.TreeBuilder;

namespace JetBrains.ReSharper.Plugins.Spring.PascalParser
{
    public class ParserCombinators
    {
    //     private static readonly Predicate<TokenNodeType> IsIdentifier = tokenNodeType => tokenNodeType.IsIdentifier;
    //
    //     private static Parser<PsiBuilder, PsiBuilder> CreateParser(
    //         Predicate<TokenNodeType> predicate,
    //         string pattern
    //     )
    //     {
    //         return Parser<PsiBuilder>.Token(builder =>
    //         {
    //             var mark = builder.Mark();
    //             var predicateResult =
    //                 predicate(builder.GetTokenType()) &&
    //                 builder.GetTokenText() == pattern;
    //
    //             if (!predicateResult)
    //             {
    //                 builder.RollbackTo(mark);
    //                 return false;
    //             }
    //
    //             builder.Drop(mark);
    //             builder.AdvanceLexer();
    //
    //             return true;
    //         });
    //     }
    //
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordAnd = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordArray = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordAs = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordAsm = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordBegin = CreateParser(IsIdentifier, "begin");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordBreak = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordCase = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordClass = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordConst = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordConstructor = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordContinue = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordDestructor = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordDiv = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordDo = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordDownTo = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordElse = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordEnd = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordFalse = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordFile = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordFor = CreateParser(IsIdentifier, "for");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordFunction = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordGoto = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordIf = CreateParser(IsIdentifier, "if");
    //
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordImplementation =
    //         CreateParser(IsIdentifier, "Implementation");
    //
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordIn = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordInline = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordInterface = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordLabel = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordMod = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordNil = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordNot = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordObject = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordOf = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordOn = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordOperator = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordOr = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordPacked = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordPrivate = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordProcedure = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordProgram = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordProtected = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordPublic = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordRecord = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordRepeat = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordSet = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordShl = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordShr = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordString = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordThen = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordTo = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordTrue = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordType = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordUnit = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordUntil = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordUses = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordVar = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordWhile = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordWith = CreateParser(IsIdentifier, "while");
    //     public static Parser<PsiBuilder, PsiBuilder> KeywordXor = CreateParser(IsIdentifier, "while");
    }
}