namespace DecisionEngine.Helpers
{
    public static class PawnHelper
    {
        public static List<int[,]> FindAllLegalMoves(int[,] board, Player player)
        {
            var result = new List<int[,]>();

            var pawnValue = player == Player.White ? 1 : -1;
            var startingRow = player == Player.White ? 1 : 6;

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (board[i, j] != pawnValue)
                        continue;

                    var newI = OneRowFoward(i, player);
                    if (!BoardHelper.IsSquareOccupied(board, newI, j))
                    {
                        var newPosition = BoardHelper.DeepCopy(board);
                        newPosition[i, j] = 0;
                        newPosition[newI, j] = pawnValue;
                        result.Add(newPosition);
                    }

                    newI = TwoRowsFoward(i, player);
                    if (i == startingRow && !BoardHelper.IsSquareOccupied(board, newI, j))
                    {
                        var newPosition = BoardHelper.DeepCopy(board);
                        newPosition[i, j] = 0;
                        newPosition[newI, j] = pawnValue;
                        result.Add(newPosition);
                    }
                }
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
