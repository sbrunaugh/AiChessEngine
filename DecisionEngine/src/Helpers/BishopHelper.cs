using DecisionEngine.Models;

namespace DecisionEngine.Helpers
{
    public static class BishopHelper
    {
        public static List<int[,]> FindAllLegalMoves(int[,] board, Player player)
        {
            var result = new List<int[,]>();

            var bishopValue = player == Player.White ? 3 : -3;

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (board[i, j] != bishopValue)
                        continue;

                    var potentialTargets = GetPotentialMoveSquares(board, new Square(i,j));

                    foreach (var x in potentialTargets)
                    {
                        // If target square is empty or has an enemy piece 
                        var validityCondition = player == Player.White
                            ? BoardHelper.IntValueAt(board, (int)x.Row, (int)x.Column) <= 0
                            : BoardHelper.IntValueAt(board, (int)x.Row, (int)x.Column) >= 0;

                        if (validityCondition)
                        {
                            var newBoard = BoardHelper.DeepCopy(board);
                            newBoard[i, j] = 0;
                            newBoard[(int)x.Row, (int)x.Column] = bishopValue;
                            result.Add(newBoard);
                        }
                    }
                }
            }

            return result;
        }

        internal static List<Square> GetPotentialMoveSquares(int[,] board, Square square)
        {
            var result = new HashSet<Square>();

            // Look towards h8
            for (var i = 1; i < 8; i++)
            {
                var newI = (int)square.Row + i;
                var newJ = (int)square.Column + i;

                if (!BoardHelper.AreValidCoords(newI, newJ))
                    break;

                result.Add(new Square(newI, newJ));

                if (BoardHelper.IntValueAt(board, newI, newJ) != 0)
                    break;
            }

            // Look towards a8
            for (var i = 1; i < 8; i++)
            {
                var newI = (int)square.Row + i;
                var newJ = (int)square.Column - i;

                if (!BoardHelper.AreValidCoords(newI, newJ))
                    break;

                result.Add(new Square(newI, newJ));

                if (BoardHelper.IntValueAt(board, newI, newJ) != 0)
                    break;
            }

            // Look towards h1
            for (var i = 1; i < 8; i++)
            {
                var newI = (int)square.Row - i;
                var newJ = (int)square.Column + i;

                if (!BoardHelper.AreValidCoords(newI, newJ))
                    break;

                result.Add(new Square(newI, newJ));

                if (BoardHelper.IntValueAt(board, newI, newJ) != 0)
                    break;
            }

            // Look towards h1
            for (var i = 1; i < 8; i++)
            {
                var newI = (int)square.Row - i;
                var newJ = (int)square.Column - i;

                if (!BoardHelper.AreValidCoords(newI, newJ))
                    break;

                result.Add(new Square(newI, newJ));

                if (BoardHelper.IntValueAt(board, newI, newJ) != 0)
                    break;
            }

            return result.ToList();
        }
    }
}
