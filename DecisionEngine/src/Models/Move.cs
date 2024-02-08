namespace DecisionEngine.Models
{
    public class Move
    {
        public Square Source { get; set; }
        public Square Destination { get; set; }
        public Player Player { get; set; }
        public Piece Piece { get; set; }
    }
}
