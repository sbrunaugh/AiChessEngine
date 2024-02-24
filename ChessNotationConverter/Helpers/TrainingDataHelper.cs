using ChessNotationConverter.Models;

namespace ChessNotationConverter.Helpers
{
    internal static class TrainingDataHelper
    {
        private static string outFile = "../../../../train_data.txt";
        
        internal static void WritePositionsToTrainingFile(List<Evaluation> evaluations)
        {
            using var sw = new StreamWriter(outFile, true);
            
            foreach (var evaluation in evaluations)
            {
                if (evaluation.Position.BlackToMove)
                {
                    evaluation.Position = evaluation.Position.Invert();
                    if (evaluation.Score != 0)
                    {
                        evaluation.Score = -evaluation.Score; // no need to invert 0
                    }
                }

                var outputStr = evaluation.Position.Serialize(true) + evaluation.Score;
                sw.WriteLine(outputStr);
            }
        }
    }
}
