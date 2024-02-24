using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChessNotationConverter.Models
{
    internal class Position
    {
        private Player playerToMove;
        internal bool WhiteToMove => playerToMove == Player.White;
        internal bool BlackToMove => playerToMove == Player.Black;
        internal int[,] Matrix { get; set; }

        public Position(Player player)
        {
            this.playerToMove = player;

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
        public Position(int[,] board, Player player)
        {
            playerToMove = player;
            Matrix = board;
        }
        public Position(Player player, Position previous, string moveStr)
        {
            playerToMove = player;
            Matrix = previous.MakeMove(moveStr);
        }

        public int[,] MakeMove(string moveStr)
        {
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

        internal Position Invert()
        {
            var newPosition = DeepCopy(Matrix);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (newPosition[i,j] != 0)
                    {
                        newPosition[i,j] = -newPosition[i,j];
                    }
                }
            }

            return new Position(newPosition, playerToMove);
        }
    }
}
