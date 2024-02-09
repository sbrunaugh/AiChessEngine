using DecisionEngine.Models;

namespace DecisionEngine.Helpers
{
    public static class BoardHelper
    {
        public static bool IsSquareOccupied(int[,] board, Square square)
        {
            return IsSquareOccupied(board, (int)square.Row, (int)square.Column);
        }
        public static bool IsSquareOccupied(int[,] board, int i, int j)
        {
            return board[i, j] != 0;
        }

        public static int IntValueAt(int[,] board, int i, int j)
        {
            return board[i, j];
        }

        public static int[,] DeepCopy(int[,] board)
        {
            var newBoard = new int[8, 8]
            {
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    newBoard[i, j] = board[i, j];
                }
            }

            return newBoard;
        }

        public static int[,] GenerateFreshBoard()
        {
            var newBoard = new int[8, 8]
            {
                // a1 ..................h1
                {  5, 2, 3, 8, 9, 3, 2, 5 },
                {  1, 1, 1, 1, 1, 1, 1, 1 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                { -1,-1,-1,-1,-1,-1,-1,-1 },
                { -5,-2,-3,-8,-9,-3,-2,-5 },
            };

            return newBoard;
        }

        public static bool AreValidCoords(int x, int y)
        {
            if (x >= 0 && x < 8 && y >= 0 && y < 8)
            {
                return true;
            }
            return false;
        }

        public static int[] ConvertBoardMatrixToArray(int[,] board)
        {
            var result = new List<int>();
            foreach (var value in board)
            {
                result.Add(value);
            }
            return result.ToArray();
        }

        public static int[,] ConvertArrayToBoardMatrix(int[] intArray)
        {
            if(intArray.Length != 64)
                throw new ArgumentException(nameof(intArray));

            var result = GenerateFreshBoard();
            for (var i = 0; i <8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    result[i, j] = intArray[(i * 8) + j];
                }
            }
            return result;
        }
    }
}
