using DecisionEngine.Models;
using System.Text;

namespace DecisionEngine.Helpers
{
    public static class MoveHelper
    {
        public static List<int[,]> FindAllLegalCastles(int[,] board, Player player)
        {
            var result = new List<int[,]>();
            var backRank = player == Player.White ? 0 : 7;
            var rookVal = player == Player.White ? 5 : -5;
            var kingVal = player == Player.White ? 9 : -9;

            // Queenside - h side
            var conditions = new List<bool>()
            {
                board[backRank, 0] == rookVal,
                board[backRank, 1] == 0,
                board[backRank, 2] == 0,
                board[backRank, 3] == 0,
                board[backRank, 4] == kingVal
            };

            if(!conditions.Any(c => c == false)) // if every condition is true
            {
                var newBoard = BoardHelper.DeepCopy(board);
                newBoard[backRank, 0] = 0;
                newBoard[backRank, 1] = 0;
                newBoard[backRank, 2] = kingVal;
                newBoard[backRank, 3] = rookVal;
                newBoard[backRank, 4] = 0;
                result.Add(newBoard);
            }

            // King side - a side
            conditions = new List<bool>()
            {
                board[backRank, 4] == kingVal,
                board[backRank, 5] == 0,
                board[backRank, 6] == 0,
                board[backRank, 7] == rookVal
            };

            if (!conditions.Any(c => c == false)) // if every condition is true
            {
                var newBoard = BoardHelper.DeepCopy(board);
                newBoard[backRank, 4] = 0;
                newBoard[backRank, 5] = rookVal;
                newBoard[backRank, 6] = kingVal;
                newBoard[backRank, 7] = 0;
                result.Add(newBoard);
            }

            return result;
        }

        internal static int PickRandomBestEvaluationIndex(List<float> evaluations, Player player)
        {
            float bestScore = player == Player.White ? evaluations.Max() : evaluations.Min();

            var indices = evaluations.Select((value, index) => new { value, index })
                .Where(pair => pair.value == bestScore)
                .Select(pair => pair.index)
                .ToList();

            var rand = new Random();
            int randomIndex = rand.Next(indices.Count);

            return indices[randomIndex];
        } 

        internal static string GenerateMoveName(int[,] originalPosition, int[,] newPosition, Player player)
        {
            var diffs = new List<BoardDifference>();

            if (diffs.Count == 4)
            {
                if (diffs.Any(d => d.j == (int)Column.a))
                    return "O-O-O";
                else
                    return "O-O";
            }

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (originalPosition[i, j] != newPosition[i, j])
                    {
                        diffs.Add(new BoardDifference(i, j, originalPosition[i, j], newPosition[i, j]));
                    }
                }
            }

            if (player == Player.White)
                diffs.OrderByDescending(d => d.newValue);
            else
                diffs.OrderBy(d => d.newValue);

            var sb = new StringBuilder();
            sb.Append(EnumHelper.PieceToChar((Piece)Math.Abs(diffs[0].newValue)));

            if (diffs[0].originalValue != 0)
                sb.Append('x');

            sb.Append(EnumHelper.ColumnToChar((Column)diffs[0].j));
            sb.Append(diffs[0].i + 1);

            return sb.ToString();
        }

        public static void FilterOutMovesResultingInCheck(List<int[,]> positions, Player player)
        {
            var enemy = player == Player.White ? Player.Black : Player.White;
            var kingValue = player == Player.White ? 9 : -9;
            var indecesToRemove = new List<int>();

            // Loop through all positions passed in
            for (var i = 0; i < positions.Count; i++)
            {
                // Find every potential move enemy can make (not filtering out checks)
                var enemeyMoves = Program.FindAllLegalMoves(positions[i], enemy, false);
                var containsCheck = false;

                // Loop through all potential positions after next move
                foreach(var position in enemeyMoves)
                {
                    // Start by assuming there is no friendly king on the board
                    var isKingPresent = false;

                    // Loop through every piece on the board
                    foreach (int pieceValue in position)
                    {
                        if (pieceValue == kingValue)
                            isKingPresent = true; // King was found
                    }

                    if(!isKingPresent)
                        containsCheck = true;
                }

                if(containsCheck)
                    indecesToRemove.Add(i);
            }

            indecesToRemove.Reverse(); // Remove later indeces first so we don't have to recount

            foreach(var index in indecesToRemove)
            {
                positions.RemoveAt(index);
            }
        }
    }
}
