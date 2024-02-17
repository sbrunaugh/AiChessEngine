namespace DecisionEngine.Models
{
    internal class InputArgs
    {
        internal int[,] Position { get; set; }
        internal Player Player {  get; set; }
        internal EvaluationType EvaluationType { get; set; }
    }
}
