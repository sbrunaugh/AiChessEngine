namespace ChessNotationConverter.Models
{
    internal class Evaluation
    {
        internal Position Position { get; private set; }
        internal float Score { get; private set; }
        internal int Hash { get; }
        internal Evaluation(Position position, float score)
        {
            Position = position;
            Score = score;
            Hash = position.Matrix.GetHashCode();
        }

    }
}
