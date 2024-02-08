namespace DecisionEngine
{
    public enum Player
    {
        White,
        Black
    }

    public enum Piece
    {
        Pawn = 1,
        Rook = 5,
        Knight = 2,
        Bishop = 3,
        Queen = 8,
        King = 9
    }

    public enum Row
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

    public enum Column
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

    public enum MoveType
    {
        Standard,
        Capture,
        Castle,
        Promotion
    }

    public static class EnumHelper
    {
        public static Piece CharToPiece(char ch)
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
        public static char PieceToChar(Piece p)
        {
            return p switch
            {
                Piece.Rook => 'R',
                Piece.Bishop => 'B',
                Piece.Knight => 'N',
                Piece.Queen => 'Q',
                Piece.King => 'N',
                _ => ' '
            };
        }

        public static int PieceAndPlayerToInt(Piece piece, Player player) 
        {
            return player == Player.White ? (int)piece : -(int)piece;
        }

        public static Column CharToColumn(char ch)
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

        public static char ColumnToChar(Column col)
        {
            return col switch
            {
                Column.a => 'a',
                Column.b => 'b',
                Column.c => 'c',
                Column.d => 'd',
                Column.e => 'e',
                Column.f => 'f',
                Column.g => 'g',
                _ => 'h'
            };
        }

        public static bool IsSquareWhite(int i, int j)
        {
            return (i+j) % 2 == 1;
        }
    }
}
