using DecisionEngine.Models;

namespace DecisionEngine.Helpers
{
    public static class KingHelper
    {
        private static readonly int[][] offsets = [
            [1, -1],
            [1, 0],
            [1, 1],
            [0, -1],
            [0, 1],
            [-1, -1],
            [-1, 0],
            [-1, 1]
        ];

        public static List<int[,]> FindAllLegalMoves(int[,] board, Player player)
        {
            var result = new List<int[,]>();

            var kingValue = player == Player.White ? 9 : -9;

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (board[i, j] != kingValue)
                        continue;

                    var potentialSquares = GetPotentialMoveSquares(board, new Square(i, j));

                    foreach (var square in potentialSquares)
                    {
                        var playerCondition = player == Player.White
                            ? BoardHelper.IntValueAt(board, (int)square.Row, (int)square.Column) <= 0
                            : BoardHelper.IntValueAt(board, (int)square.Row, (int)square.Column) >= 0;

                        if (playerCondition)
                        {
                            var newBoard = BoardHelper.DeepCopy(board);
                            newBoard[i, j] = 0;
                            newBoard[(int)square.Row, (int)square.Column] = kingValue;
                            result.Add(newBoard);
                        }
                    }
                } 
            }

            return result;
        }

        private static List<Square> GetPotentialMoveSquares(int[,] board, Square square)
        {
            var result = new List<Square>();

            foreach (var offset in offsets)
            {
                var proposedRow = (int)square.Row + offset[0];
                var proposedColumn = (int)square.Column + offset[1];

                if(BoardHelper.AreValidCoords(proposedRow, proposedColumn))
                {
                    result.Add(new Square(proposedRow, proposedColumn));
                }
            }

            return result;
        }
    }
}
