using JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexers
{
    public interface ILexemeLexer
    {
        public Lexeme Run(int currentPosition, ref string text);
    }
}