using System;

namespace YAMP
{
    [Kind(PopularKinds.Function)]
    [Description("Calculates the arithmetic mean, which is the sum of n elements divided by n.")]
    class AvgFunction : ArgumentFunction
    {
        [Description("The arithmetic mean is the naive way of averaging over a number of items. Here the arithmetic mean is taken, i.e. the sum over all items divided by the number of items.")]
        [Example("avg([1, 4, 8])", "Computes the arithmetic mean of 1, 4 and 8, i.e. (1 + 4 + 8) / 3. The result is 13 / 3 or 4.333.")]
        [Example("avg([1, 2, 3; 2, 3, 2])", "Computes the arithmetic mean over the columns, i.e. the result is a row [1.5,2.5,2.5].")]
        public Value Function(MatrixValue M)
        {
            return Average(M);
        }

        public static Value Average(MatrixValue M)
        {
            if (M.Length == 0)
                return new ScalarValue();

            if (M.IsVector)
            {
                var s = new ScalarValue();

                for (var i = 1; i <= M.Length; i++)
                    s += M[i];

                return s / M.Length;
            }
            else
            {
                var scale = 1.0;
                var s = new MatrixValue(1, M.DimensionX);

                for (var i = 1; i <= M.DimensionY; i++)
                    for (int j = 1; j <= M.DimensionX; j++)
                        s[1, j] += M[i, j];

                scale /= M.DimensionY;

                for (int j = 1; j <= s.DimensionX; j++)
                    s[1, j] *= scale;

                return s;
            }
        }
    }
}
