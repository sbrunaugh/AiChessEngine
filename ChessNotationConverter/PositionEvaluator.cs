using ChessNotationConverter.Models;

namespace ChessNotationConverter
{
    public static class PositionEvaluator
    {
        //internal static Evaluation EvaluatePosition(Game game, int positionIndex)
        //{
        //    var result = (float)0;

        //    if(game.Outcome == 0)
        //    {
        //        // for a draw, every move was pretty good
        //        result = 0.15f;
        //    }
        //    else
        //    {
        //        // early game
        //        if (positionIndex < 21)
        //        {
        //            // If the move was played, we'll say it was good
        //            result = 0.1f;
        //        }
        //        // middle/end game
        //        else
        //        {
        //            // this assumes white won
        //            // using logarithmic function to avoid late-middle game confusion
        //            //var numerator = (float)Math.Log(game.MoveCount + 1 - positionIndex);
        //            //var denominator = (float)Math.Log(game.MoveCount + 1);
        //            //result = 1 - numerator / denominator;

        //            int adjustedMoveCount = Math.Max(0, game.MoveCount - 20);
        //            int adjustedPositionIndex = Math.Max(0, positionIndex - 20);

        //            var numerator = (float)Math.Log(adjustedMoveCount + 1 - adjustedPositionIndex);
        //            var denominator = (float)Math.Log(adjustedMoveCount + 1);
        //            result = 1 - numerator / denominator;

        //            if (game.Outcome == -1)
        //            {
        //                // negate for black
        //                result = -result;
        //            }
        //        }
        //    }

        //    return new Evaluation(game.Positions[positionIndex], result);
        //}

        internal static Evaluation EvaluatePosition(Game game, int positionIndex)
        {
            var result = (float)0;

            if (game.Positions[positionIndex].WhiteToMove)
            {
                result = (float)game.WhiteElo / 3000;
            } else
            {
                result = -((float)game.BlackElo / 3000);
            }

            return new Evaluation(game.Positions[positionIndex], result);
        }
    }
}
