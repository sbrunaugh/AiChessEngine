// See https://aka.ms/new-console-template for more information
// https://www.chess.com/analysis?tab=analysis

using ChessNotationConverter;
using ChessNotationConverter.Helpers;
using ChessNotationConverter.Models;

const string filePath = "../../../../all_with_filtered_anotations_since1998.txt";

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
                // loop through all positions
                for (var i = 0; i < game.Positions.Count; i++)
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

            TrainingDataHelper.WritePositionsToTrainingFile(evaluations);

            Console.WriteLine($"Counts: {gameCount} games with {uniquePositions} unique positions written. Discarded {duplicatePositions} duplicate positions.");
            evaluations.Clear();
        }

        // convert/evaluate only 50000
        if (gameCount >= 50000)
            break;
    }
}

Console.WriteLine($"Finished converting and evaluating games.");
Console.ReadLine();