using ChessNotationConverter.Models;

namespace ChessNotationConverter
{
    public static class PositionEvaluator
    {
        internal static Evaluation EvaluatePosition(Game game, int positionIndex)
        {
            float result;

            // this assumes white won
            // using logarithmic function to avoid late-middle game confusion
            result = 1 - (float)Math.Log(game.MoveCount + 1 - positionIndex) / (float)Math.Log(game.MoveCount + 1);

            if(game.Outcome == -1) 
            {
                // negate for black
                result = -result;
            }
            else if (game.Outcome == 0)
            {
                // always 0 for draw - assume no difference in eval for white/black
                result = 0f;
            }

            return new Evaluation(game.Positions[positionIndex], result);
        }
    }
}
