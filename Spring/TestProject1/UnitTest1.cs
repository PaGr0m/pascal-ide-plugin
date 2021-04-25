using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;
using JetBrains.ReSharper.Plugins.Spring.PascalParser.Operators;
using NUnit.Framework;
using Pidgin;

namespace TestProject1
{
    public class Tests
    {

        [Test]
        public void ParserCombinatorTest()
        {
            var lexemeNot = new Lexeme("not", LexemeKind.Identifier, 0, 3);
            var lexemeAnd = new Lexeme("and", LexemeKind.Identifier, 0, 3);
            var lexemeWhile = new Lexeme("while", LexemeKind.Identifier, 0, 5);

            var list = new List<Lexeme>
            {
                lexemeNot,
                lexemeAnd,
                lexemeWhile
            };

            // Assert.
            
            // var actual = BooleanOperators.BinOperators.ParseOrThrow(list);
            // Console.WriteLine(actual.Kind);
            // Console.WriteLine(actual.Text);
        }
    }
}