using JetBrains.ReSharper.Plugins.Spring.Spring;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace JetBrains.ReSharper.Plugins.SpringTests
{
    [TestFixture]
    [TestFileExtension(".spring")]
    public class ParserTest : ParserTestBase<SpringLanguage>
    {
        protected override string RelativeTestDataPath => "parser";

        [TestCase("test01")]
        [Test]
        public void Test1(string filename)
        {
            DoOneTest(filename);
        }
    }

    public class ParserTest2
    {
        // [Test]
        // public void ParserCombinatorTest()
        // {
        //     var lexemeNot = new Lexeme("not", LexemeKind.Identifier, 0, 3);
        //     var lexemeAnd = new Lexeme("and", LexemeKind.Identifier, 0, 3);
        //     var lexemeWhile = new Lexeme("while", LexemeKind.Identifier, 0, 5);
        //
        //     var list = new List<Lexeme>
        //     {
        //         lexemeNot,
        //         // lexemeAnd,
        //         // lexemeNot
        //     };
        //
        //     var actual = BooleanOperators.BinOperators.ParseOrThrow(list);
        //     Console.WriteLine(actual.Kind);
        //     Console.WriteLine(actual.Text);
        // }
    }
}