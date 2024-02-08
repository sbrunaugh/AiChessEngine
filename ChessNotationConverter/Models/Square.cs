namespace ChessAI.Models
{
    internal class Square
    {
        internal Row Row { get; set; }
        internal Column Column { get; set; }

        internal Square(Row row, Column column)
        {
            Row = row;
            Column = column;
        }
        internal Square(int i, int j)
        {
            Row = (Row)i;
            Column = (Column)j;
        }
        internal Square(string squareStr)
        {
            Row = (Row)int.Parse(squareStr[1].ToString())-1;
            Column = EnumHelper.CharToColumn(squareStr[0]);
        }

        internal bool IsWhiteSquare()
        {
            var sum = (int)Row + (int)Column;
            return sum % 2 == 1;
        }
    }
}
