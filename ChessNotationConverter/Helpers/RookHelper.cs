using ChessNotationConverter.Models;
using System.Data.Common;

namespace ChessNotationConverter.Helpers
{
    internal static class RookHelper
    {
        internal static List<Square> GetPotentialTargetSquares(int[,] board, Player player, Square square)
        {
            var result = new List<Square>();
            var matchingValue = player == Player.White ? 5 : -5;

            // Look up
            if (square.Row != Row.Eight)
            {
                for (var i = (int)square.Row + 1; i < 8; i++)
                {
                    var whatsThere = board[i, (int)square.Column];

                    if (whatsThere == 0)
                        continue; // empty square, keep looking
                    else if (whatsThere == matchingValue)
                        result.Add(new Square(i, (int)square.Column)); // found the rook
                    else
                        break; // found another piece
                }
            }

            // Look down
            if (square.Row != Row.One)
            {
                for (var i = (int)square.Row - 1; i >= 0; i--)
                {
                    var whatsThere = board[i, (int)square.Column];

                    if (whatsThere == 0)
                        continue; // empty square, keep looking
                    else if (whatsThere == matchingValue)
                        result.Add(new Square(i, (int)square.Column)); // found the rook
                    else
                        break; // found another piece
                }
            }

            // Look right
            if (square.Column != Column.h)
            {
                for (var j = (int)square.Column + 1; j < 8; j++)
                {
                    var whatsThere = board[(int)square.Row, j];

                    if (whatsThere == 0)
                        continue; // empty square, keep looking
                    else if (whatsThere == matchingValue)
                        result.Add(new Square((int)square.Row, j)); // found the rook
                    else
                        break; // found another piece
                }
            }

            // Look left
            if (square.Column != Column.a)
            {
                for (var j = (int)square.Column - 1; j >= 0; j--)
                {
                    var whatsThere = board[(int)square.Row, j];

                    if (whatsThere == 0)
                        continue; // empty square, keep looking
                    else if (whatsThere == matchingValue)
                        result.Add(new Square((int)square.Row, j)); // found the rook
                    else
                        break; // found another piece
                }
            }

            return result;
        }

        internal static List<Square> GetPotentialTargetSquares(int[,] board, Player player, Row row)
        {
            var result = new List<Square>();
            var matchingValue = player == Player.White ? 5 : -5;

            for (var j = 0; j < 8; j++)
            {
                if (board[(int)row, j] == matchingValue)
                    result.Add(new Square((int)row, j));
            }

            return result;
        }

        internal static List<Square> GetPotentialTargetSquares(int[,] board, Player player, Column column)
        {
            var result = new List<Square>();
            var matchingValue = player == Player.White ? 5 : -5;

            for(var i = 0; i < 8; i++)
            {
                if (board[i, (int)column] == matchingValue)
                    result.Add(new Square(i, (int)column));
            }

            return result;
        }
    }
}
