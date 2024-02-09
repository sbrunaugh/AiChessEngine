using DecisionEngine.Helpers;

namespace DecisionEngine
{
    public static class Extensions
    {
        public static int[] ToIntArray(this int[,] val)
        {
            var result = new int[64];

            var i = 0;
            foreach (var entry in val)
            {
                result[i] = entry;
                i++;
            }

            return result;
        }

        public static int[,] ToIntMatrix(this int[] val) 
        {
            var result = BoardHelper.GenerateFreshBoard();

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    result[i, j] = val[(i * 8) + j];
                }
            }

            return result;
        }
    }
}
