using ChessAI.Models;

namespace ChessAI.Helpers
{
    internal static class KnightHelper
    {
        private static readonly int[][] offsets = [
            [2, -1],
            [2, 1],
            [1, -2],
            [1, 2],
            [-1, -2],
            [-1, 2],
            [-2, -1],
            [-2, 1]
        ];

        internal static List<Square> GetPotentialTargetSquares(Square square)
        {
            var result = new List<Square>();

            foreach(var offset in offsets)
            {
                var proposedRow = (int)square.Row + offset[0];
                var proposedColumn = (int)square.Column + offset[1];

                if (proposedRow >= 0 && proposedRow < 8)
                {
                    if (proposedColumn >= 0 && proposedColumn < 8)
                    {
                        result.Add(new Square(proposedRow, proposedColumn));
                    }
                }
            }

            return result;
        }
    }
}
