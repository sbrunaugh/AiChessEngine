using DecisionEngine.Models;

namespace DecisionEngine.Helpers
{
    public static class PawnHelper
    {
        public static List<Move> FindAllLegalMoves(int[,] board, Player player)
        {
            var result = new List<Move>();

            var pawnValue = player == Player.White ? 1 : -1;
            var startingRow = player == Player.White ? 1 : 6;

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (board[i, j] != pawnValue)
                        continue;

                    var targets = GetPotentialMoveSquares(board, new Square(i,j), player, i == startingRow);
                    var captureTargets = GetPotentialCaptureSquares(new Square(i, j), player);

                    foreach (var x in targets)
                    {
                        if(!BoardHelper.IsSquareOccupied(board, x))
                        {
                            var newBoard = BoardHelper.DeepCopy(board);
                            newBoard[i, j] = 0;
                            newBoard[(int)x.Row, (int)x.Column] = pawnValue;
                            var move = new Move()
                            {
                                Player = player,
                                PriorPosition = board.ToIntArray(),
                                NewPosition = newBoard.ToIntArray()
                            };
                            result.Add(move);
                        }
                    }

                    foreach (var x in captureTargets)
                    {
                        var captureCondition = player == Player.White
                            ? BoardHelper.IntValueAt(board, (int)x.Row, (int)x.Column) < 0
                            : BoardHelper.IntValueAt(board, (int)x.Row, (int)x.Column) > 0;
                        
                        if (captureCondition)
                        {
                            var newBoard = BoardHelper.DeepCopy(board);
                            newBoard[i, j] = 0;
                            newBoard[(int)x.Row, (int)x.Column] = pawnValue;
                            var move = new Move()
                            {
                                Player = player,
                                PriorPosition = board.ToIntArray(),
                                NewPosition = newBoard.ToIntArray()
                            };
                            result.Add(move);
                        }
                    }
                }
            }

            return result;
        }

        private static List<Square> GetPotentialMoveSquares(int[,] board, Square square, Player player, bool isStartingRow = false)
        {
            var result = new List<Square>();
            var oneUpSquare = new Square(OneRowFoward((int)square.Row, player), (int)square.Column);
            result.Add(oneUpSquare);

            // Can't jump over other pieces
            if(isStartingRow && !BoardHelper.IsSquareOccupied(board, oneUpSquare))
            {
                var twoUpSquare = new Square(TwoRowsFoward((int)square.Row, player), (int)square.Column);
                result.Add(twoUpSquare);
            }

            return result;
        }

        private static List<Square> GetPotentialCaptureSquares(Square square, Player player)
        {
            var rowMod = player == Player.White ? 1 : -1;
            var result = new List<Square>();

            if (BoardHelper.AreValidCoords((int)square.Row + rowMod, (int)square.Column - 1))
            {
                result.Add(new Square((int)square.Row + rowMod, (int)square.Column - 1));
            }

            if (BoardHelper.AreValidCoords((int)square.Row + rowMod, (int)square.Column + 1))
            {
                result.Add(new Square((int)square.Row + rowMod, (int)square.Column + 1));
            }

            return result;
        }

        private static int OneRowFoward(int i, Player player)
        {
            return player == Player.White ? i + 1 : i - 1;
        }
        private static int TwoRowsFoward(int i, Player player)
        {
            return player == Player.White ? i + 2 : i - 2;
        }
    }
}
