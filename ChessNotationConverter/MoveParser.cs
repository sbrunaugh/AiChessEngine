using ChessNotationConverter.Helpers;
using ChessNotationConverter.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessNotationConverter
{
    internal static class MoveParser
    {
        internal static int[,] ParseMove(int[,] board, Player player, MoveType moveType, string move) 
        {
            if (move.Contains('+') || move.Contains("#")) // Check & checkmate operators
            {
                move = move.Replace("+", string.Empty);
                move = move.Replace("#", string.Empty);
            }

            // Promotion
            if(moveType == MoveType.Promotion)
            {
                var moveParts = move.Split('=');
                Assert.AreEqual(2, moveParts.Length);

                var promoDestinationStr = moveParts[0].Substring(moveParts[0].Length - 2);
                var promoDestination = new Square(promoDestinationStr);

                Square promoSource;
                if (moveParts[0].Contains('x')) // Capture and promotion at the same time
                {
                    promoSource = DeduceCaptureSource(board, player, Piece.Pawn, promoDestination, moveParts[0]);
                }
                else
                {
                    promoSource = DeduceSource(board, player, Piece.Pawn, promoDestination, moveParts[0]);
                }

                var promoTargetValue = EnumHelper.PieceAndPlayerToInt(EnumHelper.CharToPiece(moveParts[1][0]), player);
                board[(int)promoDestination.Row, (int)promoDestination.Column] = promoTargetValue;
                board[(int)promoSource.Row, (int)promoSource.Column] = 0;

                return board;
            }

            // Castling
            if(moveType == MoveType.Castle)
            {
                var row = player == Player.White ? 0 : 7;
                var rookInt = player == Player.White ? 5 : -5;
                var kingInt = player == Player.White ? 9 : -9;

                if (move == "O-O") // King side
                {
                    board[row, 4] = 0;
                    board[row, 5] = rookInt;
                    board[row, 6] = kingInt;
                    board[row, 7] = 0;
                }
                else if (move == "O-O-O") // Queen side
                {
                    board[row, 0] = 0;
                    board[row, 1] = 0;
                    board[row, 2] = kingInt;
                    board[row, 3] = rookInt;
                    board[row, 4] = 0;
                }
                else
                {
                    throw new NotImplementedException();
                }

                return board;
            }

            var piece = EnumHelper.CharToPiece(move[0]);
            var destinationStr = move.Contains('+') // Check symbol
                ? move.Substring(move.Length - 3, move.Length - 1) 
                : move.Substring(move.Length - 2);
            var destination = new Square(destinationStr);
            var source = moveType switch
            {
                MoveType.Standard => DeduceSource(board, player, piece, destination, move),
                MoveType.Capture => DeduceCaptureSource(board, player, piece, destination, move),
                _ => throw new NotImplementedException()
            };

            var targetValue = EnumHelper.PieceAndPlayerToInt(piece, player);

            //en peasant check
            if (piece == Piece.Pawn 
                && moveType == MoveType.Capture 
                && board[(int)destination.Row, (int)destination.Column] == 0)
            {
                if(player == Player.White)
                {
                    board[(int)destination.Row - 1, (int)destination.Column] = 0;
                } else
                {
                    board[(int)destination.Row + 1, (int)destination.Column] = 0;
                }
            }

            board[(int)destination.Row, (int)destination.Column] = targetValue;
            board[(int)source.Row, (int)source.Column] = 0;

            return board;
        }

        private static Square DeduceSource(int[,] board, Player player, Piece piece, Square destination, string moveStr)
        {
            var targetValue = EnumHelper.PieceAndPlayerToInt(piece, player);

            if (piece == Piece.Pawn)
            {
                if(player == Player.White)
                {
                    var oneBack = board[(int)(destination.Row - 1), (int)destination.Column];
                    var twoBack = board[(int)(destination.Row - 2), (int)destination.Column];

                    if (oneBack != targetValue && twoBack != targetValue)
                        throw new Exception("Not seeing pawn at either source square");

                    return oneBack == targetValue
                        ? new Square(destination.Row - 1, destination.Column)
                        : new Square(destination.Row - 2, destination.Column);
                }
                else
                {
                    var oneUp = board[(int)(destination.Row + 1), (int)destination.Column];
                    var twoUp = board[(int)(destination.Row + 2), (int)destination.Column];

                    if (oneUp != targetValue && twoUp != targetValue)
                        throw new Exception("Not seeing pawn at either source square");

                    return oneUp == targetValue
                        ? new Square(destination.Row + 1, destination.Column)
                        : new Square(destination.Row + 2, destination.Column);
                }
            }
            else if (piece == Piece.Rook)
            {
                List<Square> potentialSquares;
                // Column or row in move string
                if (moveStr.Length == 4)
                {
                    var extraChar = moveStr[1];
                    var isRow = char.IsNumber(extraChar);

                    potentialSquares = isRow
                        ? RookHelper.GetPotentialTargetSquares(board, player, (Row)int.Parse(extraChar.ToString()) - 1) // subtract 1 b/c 0-index
                        : RookHelper.GetPotentialTargetSquares(board, player, EnumHelper.CharToColumn(extraChar));
                }
                else
                {
                    potentialSquares = RookHelper.GetPotentialTargetSquares(board, player, destination);
                }

                foreach (var square in potentialSquares)
                {
                    if (board.AtSquare(square) == targetValue)
                        return square;
                }

                throw new Exception("Rook not found on the board");
            }
            else if (piece == Piece.Knight)
            {
                // Column in move string - https://en.wikipedia.org/wiki/Portable_Game_Notation#Movetext
                if (moveStr.Length == 4)
                {
                    //var c = EnumHelper.CharToColumn(moveStr[1]);

                    var extraChar = moveStr[1];
                    var isRow = char.IsNumber(extraChar);

                    if(isRow)
                    {
                        var i = int.Parse(extraChar.ToString()) - 1; // subtract one b/c 0-index
                        for (int j = 0; j < 8; j++)
                        {
                            if (board[i, j] == targetValue)
                                return new Square(i, j);
                        }
                    } else
                    {
                        var j = EnumHelper.CharToColumn(extraChar);
                        for (int i = 0; i < 8; i++)
                        {
                            if (board[i, (int)j] == targetValue)
                                return new Square(i, (int)j);
                        }
                    }


                }

                // TODO: fix known issue where when one knight is pinned, the other has to move,
                // but the notation won't specify the origin of the moving knight since the pin
                // is implicitely understood by the players.

                var potentialSquares = KnightHelper.GetPotentialTargetSquares(destination);

                foreach(var square in potentialSquares)
                {
                    if (board.AtSquare(square) == targetValue)
                    {
                        return new Square(square.Row, square.Column);
                    }
                }

                throw new Exception("No source detected for knight move");
            }
            else if (piece == Piece.Bishop)
            {
                // light or dark square bishop?
                var isLightSquare = destination.IsWhiteSquare();

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (EnumHelper.IsSquareWhite(i, j) == isLightSquare && board[i, j] == targetValue)
                            return new Square(i, j);
                    }
                }

                throw new Exception("Couldn't find the bishop");
            }
            else if (piece == Piece.King)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board[i, j] == targetValue)
                            return new Square(i, j);
                    }
                }
            }
            else if (piece == Piece.Queen)
            {
                // Assume only one queen on the board
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board[i, j] == targetValue)
                            return new Square(i, j);
                    }
                }
            }

            throw new NotImplementedException();
        }

        private static Square DeduceCaptureSource(int[,] board, Player player, Piece piece, Square destination, string moveStr)
        {
            if (moveStr.Length != 4 && moveStr.Length != 5)
                throw new Exception("unexpected capture move string");

            var targetValue = EnumHelper.PieceAndPlayerToInt(piece, player);

            if (piece == Piece.Pawn)
            {
                var sourceColumn = EnumHelper.CharToColumn(moveStr[0]);

                var sourceRow = player == Player.White ? destination.Row - 1 : destination.Row + 1;
                return new Square(sourceRow, sourceColumn);
            }
            else if (piece == Piece.Rook)
            {
                List<Square> potentialSquares;
                // Column or row in move string
                if (moveStr.Length == 5)
                {
                    var extraChar = moveStr[1];
                    var isRow = char.IsNumber(extraChar);

                    potentialSquares = isRow
                        ? RookHelper.GetPotentialTargetSquares(board, player, (Row)int.Parse(extraChar.ToString()))
                        : RookHelper.GetPotentialTargetSquares(board, player, EnumHelper.CharToColumn(extraChar));
                }
                else
                {
                    potentialSquares = RookHelper.GetPotentialTargetSquares(board, player, destination);
                }

                foreach (var square in potentialSquares)
                {
                    if (board.AtSquare(square) == targetValue)
                        return square;
                }

                throw new Exception("Rook not found on the board");
            }
            else if (piece == Piece.Knight)
            {
                if (moveStr.Length != 4 && moveStr.Length != 5)
                    throw new Exception("invalid move string");

                var potentialSquares = KnightHelper.GetPotentialTargetSquares(destination);

                foreach (var square in potentialSquares)
                {
                    var asdf = board.AtSquare(square);
                    if (board.AtSquare(square) == targetValue)
                    {
                        return new Square(square.Row, square.Column);
                    }
                }

                throw new Exception("No source detected for knight move");
            }
            else if (piece == Piece.Bishop)
            {
                // light or dark square bishop?
                var isLightSquare = destination.IsWhiteSquare();

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (EnumHelper.IsSquareWhite(i, j) == isLightSquare && board[i, j] == targetValue)
                            return new Square(i, j);
                    }
                }

                throw new Exception("Couldn't find the bishop");
            }
            else if (piece == Piece.Queen)
            {
                // Assume only one queen on the board
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board[i, j] == targetValue)
                            return new Square(i, j);
                    }
                }
            }
            else if (piece == Piece.King)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board[i, j] == targetValue)
                            return new Square(i, j);
                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}
