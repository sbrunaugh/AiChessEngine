﻿using DecisionEngine.Models;

namespace DecisionEngine.Helpers
{
    public static class QueenHelper
    {
        public static List<int[,]> FindAllLegalMoves(int[,] board, Player player)
        {
            var result = new List<int[,]>();

            var queenkValue = player == Player.White ? 8 : -8;

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (board[i, j] != queenkValue)
                        continue;

                    var potentialTargets = RookHelper.GetPotentialMoveSquares(board, new Square(i, j));
                    potentialTargets.AddRange(BishopHelper.GetPotentialMoveSquares(board, new Square(i, j)));

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
                            newBoard[(int)x.Row, (int)x.Column] = queenkValue;
                            result.Add(newBoard);
                        }
                    }
                }
            }

            return result;
        }
    }
}