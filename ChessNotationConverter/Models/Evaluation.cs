namespace ChessNotationConverter.Models
{
    internal class Evaluation
    {
        internal Position Position { get; set; }
        internal float Score { get; set; }
        internal int Hash { get; }
        internal Evaluation(Position position, float score)
        {
            Position = position;
            Score = score;
            Hash = position.Matrix.GetHashCode();
        }

    }
}
