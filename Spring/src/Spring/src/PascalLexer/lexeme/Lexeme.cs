using System.Collections.Generic;
using System.Text;

namespace JetBrains.ReSharper.Plugins.Spring.PascalLexer.lexeme
{
    public class Lexeme
    {
        public readonly string Text;
        public readonly LexemeKind Kind;
        public readonly int StartPosition;
        public readonly int FinishPosition;

        public Lexeme(string text, LexemeKind kind, int startPosition, int finishPosition)
        {
            Text = text;
            Kind = kind;
            StartPosition = startPosition;
            FinishPosition = finishPosition;
        }

        private sealed class LexemeEqualityComparer : IEqualityComparer<Lexeme>
        {
            public bool Equals(Lexeme x, Lexeme y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Text == y.Text && x.Kind == y.Kind && x.StartPosition == y.StartPosition &&
                       x.FinishPosition == y.FinishPosition;
            }

            public int GetHashCode(Lexeme obj)
            {
                unchecked
                {
                    var hashCode = (obj.Text != null ? obj.Text.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (int) obj.Kind;
                    hashCode = (hashCode * 397) ^ obj.StartPosition;
                    hashCode = (hashCode * 397) ^ obj.FinishPosition;
                    return hashCode;
                }
            }
        }

        public static IEqualityComparer<Lexeme> LexemeComparer { get; } = new LexemeEqualityComparer();

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Text).Append(" ");
            stringBuilder.Append(Kind).Append(" ");
            stringBuilder.Append(StartPosition).Append(" ");
            stringBuilder.Append(FinishPosition);

            return stringBuilder.ToString();
        }
    }
}