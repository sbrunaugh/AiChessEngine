using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace ChessNotationConverter.Models
{
    internal class Position
    {
        internal int[,] Matrix { get; set; }

        public Position()
        {
            // Starting position
            Matrix = new int[8, 8]
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
        }
        public Position(Position previous, string moveStr)
        {
            Matrix = previous.MakeMove(moveStr);
        }

        public int[,] MakeMove(string moveStr)
        {
            //Console.Clear();
            //this.Print();
            //Console.WriteLine($"\nParsing move {moveStr}.");
             
            var moveSplit = moveStr.Split(".");

            if (moveSplit.Length != 2)
                throw new ArgumentException(nameof(moveStr));

            var player = moveSplit[0][0] == 'W' ? Player.White : Player.Black;
            var move = moveSplit[1];

            //W1.e4 B1.d5 W2.exd5 B2.Qxd5 W3.Nc3 B3.Qa5 W4.d4 B4.Nf6 W5.Nf3 B5.c6 W6.Ne5 B6.Bf5

            var moveType = MoveType.Standard;
            if (move.Contains("="))
                moveType = MoveType.Promotion; // could be a capture and promotion, so this condition is first
            else if (move.Contains("O"))
                moveType = MoveType.Castle;
            else if (move.Contains("x"))
                moveType = MoveType.Capture;


            var copiedMatrix = DeepCopy(Matrix);
            return MoveParser.ParseMove(copiedMatrix, player, moveType, move);
        }

        private void Print()
        {
            for (var row = 7; row >= 0; row--)
            {
                var sb = new StringBuilder();
                for (var j = 0; j < 8; j++)
                {
                    var msg = Matrix[row, j] < 0
                        ? Matrix[row, j].ToString()
                        : " " + Matrix[row, j];

                    sb.Append(msg);
                }
                Console.WriteLine(sb.ToString());
            }
        }

        private int[,] DeepCopy(int[,] board)
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

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    newBoard[i, j] = board[i, j];
                }
            }

            return newBoard;
        } 

        internal string Serialize(bool trailingComma = true)
        {
            var sb = new StringBuilder();
            foreach (var pieceVal in Matrix)
            {
                sb.Append(pieceVal + ",");
            }

            return trailingComma ? sb.ToString() : sb.ToString().TrimEnd(',');
        }
    }
}
