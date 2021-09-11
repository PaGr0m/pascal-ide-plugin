using NUnit.Framework;

namespace ParseCombinatorTest.Tests
{
    public class Tests
    {
        [Test]
        public void ParserCombinatorTest()
        {
            Assert.True(char.IsWhiteSpace(' '));
            Assert.True(char.IsWhiteSpace(' '));
            Assert.True(char.IsWhiteSpace('\n'));
            Assert.True(char.IsWhiteSpace('\t'));
        //     var lexemeNot = new Lexeme("not", LexemeKind.Identifier, 0, 3);
        //     var lexemeAnd = new Lexeme("and", LexemeKind.Identifier, 0, 3);
        //     var lexemeWhile = new Lexeme("while", LexemeKind.Identifier, 0, 5);
        //
        //     BooleanOperators.BinOperators.Parse(new List<Lexeme> {lexemeNot});
        //
        //     
        //     Assert.AreEqual(
        //         lexemeNot,
        //         BooleanOperators.BinOperators.ParseOrThrow(new List<Lexeme> {lexemeNot})
        //     );
        //
        //     Assert.AreEqual(
        //         lexemeAnd,
        //         BooleanOperators.BinOperators.ParseOrThrow(new List<Lexeme> {lexemeAnd})
        //     );
        //
        //     Assert.Throws<ParseException>(() =>
        //         BooleanOperators.BinOperators.ParseOrThrow(new List<Lexeme> {lexemeWhile})
        //     );
        }
    }
}