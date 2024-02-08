﻿using ChessAI.Models;

namespace ChessAI
{
    internal static class Extensions
    {
        public static int AtSquare(this int[,] val, Square square)
        {
            var i = (int)square.Row;
            var j = (int)square.Column;
            return val[i, j];
        }
    }
}
