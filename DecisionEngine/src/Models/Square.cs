namespace DecisionEngine.Models
{
    public class Square
    {
        public Row Row { get; set; }
        public Column Column { get; set; }

        public Square(Row row, Column column)
        {
            Row = row;
            Column = column;
        }
        public Square(int i, int j)
        {
            Row = (Row)i;
            Column = (Column)j;
        }
        public Square(string squareStr)
        {
            Row = (Row)int.Parse(squareStr[1].ToString())-1;
            Column = EnumHelper.CharToColumn(squareStr[0]);
        }

        public bool IsWhiteSquare()
        {
            var sum = (int)Row + (int)Column;
            return sum % 2 == 1;
        }
    }
}
