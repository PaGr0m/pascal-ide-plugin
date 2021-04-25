namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme
{
    public enum LexemeKind
    {
        // Numbers
        HexNumber,
        OctalNumber,
        BinaryNumber,
        Integer,
        Real,

        // Comment
        SingleComment,
        MultipleComment,
        
        // Special character
        SingleSpecialCharacter,
        PairSpecialCharacter,
        
        // Strings
        ControlString,
        QuotedString,
        
        Identifier,
        Keyword,
        Whitespace,
    }
}