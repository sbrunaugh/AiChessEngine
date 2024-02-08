namespace ChessNotationConverter.Models
{
    internal class Game
    {
        // Given data
        internal DateTime Date { get; set; } = DateTime.MinValue;
        internal int Outcome { get; set; } // 1 = white won, 0 = draw, -1 = black won
        internal int WhiteElo { get; set; }
        internal int BlackElo { get; set; }
        internal int MoveCount { get; set; }
        internal string PgnText { get; set; }

        // Computed data
        internal List<string> Moves { get; set; }
        internal List<Position> Positions { get; set; } = new List<Position>() { new Position() };
        
        public Game(string gameText) 
        {
            var split = gameText.Split(" ### ");

            if(split.Length != 2 )
            {
                throw new ArgumentException(nameof(gameText));
            }

            //1 2000.03.14 1-0 2851 None 67 date_false result_false welo_false belo_true edate_true setup_false fen_false result2_false oyrange_false blen_false
            var metaSplit = split[0].Split(" ");

            if(metaSplit.Length != 16)
            {
                throw new ArgumentException(nameof(gameText));
            }

            if(DateTime.TryParse(metaSplit[1], out var date))
            {
                Date = date;
            }

            if (metaSplit[2] == "1-0")
                Outcome = 1;
            else if (metaSplit[2] == "0-1")
                Outcome = -1;
            else 
                Outcome = 0;

            WhiteElo = metaSplit[3] == "None" ? -1 : int.Parse(metaSplit[3]);
            BlackElo = metaSplit[3] == "None" ? -1 : int.Parse(metaSplit[3]);
            MoveCount = int.Parse(metaSplit[5]);

            if (MoveCount < 2)
                throw new Exception("game is less than 2 moves");

            //W1.e4 B1.d5 W2.exd5 B2.Qxd5 W3.Nc3 B3.Qa5 W4.d4 B4.Nf6 W5.Nf3 B5.c6 W6.Ne5 B6.Bf5 W7.g4 B7.Be4

            PgnText = split[1].TrimEnd();
            Moves = PgnText.Split(" ").ToList();

            for (int i = 0; i < Moves.Count; i++)
            {
                var prev = Positions[i]; // Always one more position than move
                Positions.Add(new Position(prev, Moves[i]));
            }

            //Console.Clear();
            //Console.WriteLine("Finished parsing game.");
        }

        internal double GetOutcomeForPositionIndex(int index)
        {
            if(Outcome == 0)
            {
                return 0.0;
            } 

            var absoluteVal = (double)index / (double)MoveCount;

            return Outcome == 1
                ? absoluteVal
                : -absoluteVal;
        }
    }
}
