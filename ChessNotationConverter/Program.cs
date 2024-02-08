// See https://aka.ms/new-console-template for more information
// https://www.chess.com/analysis?tab=analysis

using ChessNotationConverter.Models;
using System.Text;

const string filePath = "C:\\Users\\bruna\\Downloads\\all_with_filtered_anotations_since1998.txt\\all_with_filtered_anotations_since1998.txt";
string line;
int lineNumber = 0;
var games = new List<Game>();
int gameCount = 0;
int positionCount = 0;
int whiteWins = 0;
int draws = 0;
int blackWins = 0;

using (StreamReader file = new StreamReader(filePath))
{
    while ((line = file.ReadLine()) != null)
    {
        lineNumber++;
        if (lineNumber >= 6)
        {
            string currentLine = line;

            Game game;
            try
            {
                game = new Game(line);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Something went wrong while parsing out the game:\n" + ex.ToString());
                continue;
            }
            
            games.Add(game);
        }

        if (games.Count >= 100)
        {
            gameCount += 100;
            positionCount += games.Sum(g => g.Positions.Count);
            whiteWins += games.Where(g => g.Outcome == 1).Count();
            draws += games.Where(g => g.Outcome == 0).Count();
            blackWins += games.Where(g => g.Outcome == -1).Count();

            Console.WriteLine($"Counts: {gameCount} games with {positionCount} positions written. {whiteWins} white wins - {draws} draws - {blackWins} black wins.");

            var outputFilePath = Environment.CurrentDirectory + "../../../../train_data.csv";

            using (StreamWriter sw = new StreamWriter(outputFilePath, true))
            {
                // loop through 100 games
                foreach (var game in games)
                {
                    if (game.WhiteElo < 2000 && game.BlackElo < 2000)
                    {
                        continue;
                    }

                    // loop through all positions
                    for (var i = 1; i < game.Positions.Count; i++)
                    {
                        var sb = new StringBuilder();
                        foreach (var pieceInt in game.Positions[i].Matrix)
                        {
                            sb.Append(pieceInt.ToString() + ",");
                        }
                        sb.Append(game.GetOutcomeForPositionIndex(i).ToString() + "\n");

                        sw.WriteLine(sb.ToString());
                    }

                }
            }

            games.Clear();
        }
    }
}

Console.WriteLine($"finished parsing games");
Console.ReadLine();