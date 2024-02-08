namespace DecisionEngine.Models
{
    internal class BoardDifference
    {
        internal int i { get; set; }
        internal int j { get; set; }
        internal int originalValue { get; set; }
        internal int newValue { get; set; }

        public BoardDifference(int i, int j, int og, int n)
        {
            this.i = i;
            this.j = j;
            this.originalValue = og;
            this.newValue = n;
        }
    }
}
