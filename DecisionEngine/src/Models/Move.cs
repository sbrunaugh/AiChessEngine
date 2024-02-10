using System.Text;

namespace DecisionEngine.Models
{
    public class Move
    {
        public Player Player { get; set; }
        public int[] PriorPosition { get; set; }
        public int[] NewPosition { get; set; }
        internal string MoveName
        {
            get
            {
                var diffs = new List<BoardDifference>();

                for (var i = 0; i < 8; i++)
                {
                    for (var j = 0; j < 8; j++)
                    {
                        if (PriorPosition.ToIntMatrix()[i, j] != NewPosition.ToIntMatrix()[i, j])
                        {
                            diffs.Add(new BoardDifference(i, j, PriorPosition.ToIntMatrix()[i, j], NewPosition.ToIntMatrix()[i, j]));
                        }
                    }
                }

                if (diffs.Count == 4)
                {
                    if (diffs.Any(d => d.j == (int)Column.a))
                        return "O-O-O";
                    else
                        return "O-O";
                }

                diffs = Player == Player.White
                    ? diffs.OrderByDescending(d => d.newValue).ToList()
                    : diffs.OrderBy(d => d.newValue).ToList();

                var sb = new StringBuilder();
                sb.Append(EnumHelper.PieceToChar((Piece)Math.Abs(diffs[0].newValue)));

                if (diffs[0].originalValue != 0)
                    sb.Append('x');

                sb.Append(EnumHelper.ColumnToChar((Column)diffs[0].j));
                sb.Append(diffs[0].i + 1);

                return sb.ToString();
            }
        }
    }
}
