// See https://aka.ms/new-console-template for more information
// https://www.chess.com/analysis?tab=analysis

using ChessNotationConverter;
using ChessNotationConverter.Models;

const string filePath = "C:\\Users\\bruna\\Downloads\\all_with_filtered_anotations_since1998.txt\\all_with_filtered_anotations_since1998.txt";
const string outputFilePath = "../../../../train_data.csv";
string line;
int lineNumber = 0;
var games = new List<Game>();
var evaluations = new List<Evaluation>();
var discardHashes = new List<int>();
int gameCount = 0;
int uniquePositions = 0;
int duplicatePositions = 0;

using (var file = new StreamReader(filePath))
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

        if (games.Count >= 200)
        {
            gameCount += 200;

            // loop through 1000 games
            foreach (var game in games)
            {
                if (game.WhiteElo < 2000 && game.BlackElo < 2000)
                {
                    gameCount--;
                    continue;
                }

                // loop through all positions
                for (var i = 1; i < game.Positions.Count; i++)
                {
                    var evaluation = PositionEvaluator.EvaluatePosition(game, i);
                    // if no duplicates in current batch and no matches in known duplicates
                    if (!evaluations.Any(e => e.Hash == evaluation.Hash) && !discardHashes.Any(x => x == evaluation.Hash))
                    {
                        uniquePositions++;
                        evaluations.Add(evaluation);
                    } else
                    {
                        duplicatePositions++;
                        gameCount--;
                    }
                }
            }

            games.Clear();

            using (var sw = new StreamWriter(outputFilePath, true))
            {
                foreach(var evaluation in evaluations)
                {
                    var outputStr = evaluation.Position.Serialize(true) + evaluation.Score;
                    sw.WriteLine(outputStr);
                }
            }

            Console.WriteLine($"Counts: {gameCount} games with {uniquePositions} unique positions written. Discarded {duplicatePositions} duplicate positions.");
            evaluations.Clear();
        }

        if (gameCount >= 1000000)
            break;
    }
}

Console.WriteLine($"Finished converting and evaluating games.");
Console.ReadLine();