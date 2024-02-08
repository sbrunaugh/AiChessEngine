using DecisionEngine.Models;

namespace DecisionEngine.Helpers
{
    public static class KnightHelper
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

        public static List<int[,]> FindAllLegalMoves(int[,] board, Player player)
        {
            var result = new List<int[,]>();

            var knightValue = player == Player.White ? 2 : -2;

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (board[i, j] != knightValue)
                        continue;

                    var potentialTargets = GetPotentialJumpSquares(new Square(i, j));

                    foreach (var x in potentialTargets)
                    {
                        if(player == Player.White)
                        {
                            // If target square is empty or has a black piece
                            if (BoardHelper.IntValueAt(board, (int)x.Row, (int)x.Column) <= 0)
                            {
                                var newBoard = BoardHelper.DeepCopy(board);
                                newBoard[i, j] = 0;
                                newBoard[(int)x.Row, (int)x.Column] = knightValue;
                                result.Add(newBoard);
                            }
                        }
                        else
                        {
                            // If target square is empty or has a white piece
                            if (BoardHelper.IntValueAt(board, (int)x.Row, (int)x.Column) >= 0)
                            {
                                var newBoard = BoardHelper.DeepCopy(board);
                                newBoard[i, j] = 0;
                                newBoard[(int)x.Row, (int)x.Column] = knightValue;
                                result.Add(newBoard);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static List<Square> GetPotentialJumpSquares(Square square)
        {
            var result = new List<Square>();

            foreach (var offset in offsets)
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
