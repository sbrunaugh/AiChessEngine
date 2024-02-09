namespace DecisionEngine.Models
{
    public class FutureCalculation
    {
        public int[] Position { get; set; }
        public List<EvaluatedMove> NextMoves { get; set; }
        internal (float, float) MinAndMaxEvals()
        {
            if (NextMoves == null || NextMoves.Count < 1) 
            { 
                throw new ArgumentException(nameof(NextMoves));
            }

            var minValue = NextMoves.Min(nm => nm.Evaluation);
            var maxValue = NextMoves.Max(nm => nm.Evaluation);

            return (minValue, maxValue);
        }
    }
}
