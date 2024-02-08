using DecisionEngine.Models;

namespace DecisionEngine.Helpers
{
    public static class RookHelper
    {
        public static List<int[,]> FindAllLegalMoves(int[,] board, Player player)
        {
            var result = new List<int[,]>();

            var rookValue = player == Player.White ? 5 : -5;

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (board[i, j] != rookValue)
                        continue;

                    var potentialTargets = GetPotentialMoveSquares(board, new Square(i, j));

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
                            newBoard[(int)x.Row, (int)x.Column] = rookValue;
                            result.Add(newBoard);
                        }
                    }
                }
            }
            
            return result;
        }

        internal static List<Square> GetPotentialMoveSquares(int[,] board, Square square)
        {
            var result = new List<Square>();

            // Look towards row 8
            for (var i = 1; i < 8; i++)
            {
                var newI = (int)square.Row + i;

                if (!BoardHelper.AreValidCoords(newI, (int)square.Column))
                    break;

                result.Add(new Square(newI, (int)square.Column));

                if (BoardHelper.IntValueAt(board, newI, (int)square.Column) != 0)
                    break;
            }

            // Look towards row 1
            for (var i = 1; i < 8; i++)
            {
                var newI = (int)square.Row - i;

                if (!BoardHelper.AreValidCoords(newI, (int)square.Column))
                    break;

                result.Add(new Square(newI, (int)square.Column));

                if (BoardHelper.IntValueAt(board, newI, (int)square.Column) != 0)
                    break;
            }

            // Look towards column h
            for (var i = 1; i < 8; i++)
            {
                var newJ = (int)square.Column + i;

                if (!BoardHelper.AreValidCoords((int)square.Row, newJ))
                    break;

                result.Add(new Square((int)square.Row, newJ));

                if (BoardHelper.IntValueAt(board, (int)square.Row, newJ) != 0)
                    break;
            }

            // Look towards column a
            for (var i = 1; i < 8; i++)
            {
                var newJ = (int)square.Column - i;

                if (!BoardHelper.AreValidCoords((int)square.Row, newJ))
                    break;

                result.Add(new Square((int)square.Row, newJ));

                if (BoardHelper.IntValueAt(board, (int)square.Row, newJ) != 0)
                    break;
            }

            return result;
        }
    }
}
