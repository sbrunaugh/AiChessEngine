namespace ChessNotationConverter
{
    internal enum Player
    {
        White,
        Black
    }

    internal enum Piece
    {
        Pawn = 1,
        Rook = 5,
        Knight = 2,
        Bishop = 3,
        Queen = 8,
        King = 9
    }

    internal enum Row
    {
        One = 0,
        Two = 1,
        Three = 2,
        Four = 3,
        Five = 4,
        Six = 5,
        Seven = 6,
        Eight = 7
    }

    internal enum Column
    {
        a = 0,
        b = 1,
        c = 2,
        d = 3,
        e = 4,
        f = 5,
        g = 6,
        h = 7
    }

    internal enum MoveType
    {
        Standard,
        Capture,
        Castle,
        Promotion
    }

    internal static class EnumHelper
    {
        internal static Piece CharToPiece(char ch)
        {
            return ch switch
            {
                'R' => Piece.Rook,
                'B' => Piece.Bishop,
                'N' => Piece.Knight,
                'Q' => Piece.Queen,
                'K' => Piece.King,
                _ => Piece.Pawn
            };
        }

        internal static int PieceAndPlayerToInt(Piece piece, Player player) 
        {
            return player == Player.White ? (int)piece : -(int)piece;
        }

        internal static Column CharToColumn(char ch)
        {
            return ch switch
            {
                'a' => Column.a,
                'b' => Column.b,
                'c' => Column.c,
                'd' => Column.d,
                'e' => Column.e,
                'f' => Column.f,
                'g' => Column.g,
                'h' => Column.h,
                _ => throw new ArgumentException("char not a column", nameof(ch))
            };
        }

        internal static bool IsSquareWhite(int i, int j)
        {
            return (i+j) % 2 == 1;
        }
    }
}
