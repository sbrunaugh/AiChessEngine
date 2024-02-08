namespace ChessNotationConverter.Models
{
    internal class Move
    {
        internal Square Source { get; set; }
        internal Square Destination { get; set; }
        internal Player Player { get; set; }
        internal Piece Piece { get; set; }
    }
}
