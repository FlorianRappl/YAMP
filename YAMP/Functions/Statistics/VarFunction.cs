using System;

namespace YAMP
{
    [Description("In probability theory and statistics, the variance is a measure of how far a set of numbers is spread out.")]
    [Kind(PopularKinds.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Variance")]
    class VarFunction : ArgumentFunction
    {
        [Description("The variance is obtained by calculating the data set consisting of each data point in the original data set subtracting the arithmetic mean for the data set, and then squaring it.")]
        [Example("var([1, 2, 3, 4, 5, 6])", "Gives the variance for throwing a perfect sided die, which is roughly 2.9.")]
        [Example("var([1, 2, 3; 4, 5, 6; 7, 8, 9])", "Computes the variance for each column of the given matrix.")]
        public Value Function(MatrixValue M)
        {
            return Variance(M);
        }

        public static Value Variance(MatrixValue M)
        {
            if (M.Length == 0)
                return new ScalarValue();

            if (M.IsVector)
            {
                var variance = new ScalarValue();
                var mean = M.Sum() / M.Length;

                for (int i = 1; i <= M.Length; i++)
                    variance += (M[i] - mean).Square();

                return variance / M.Length;
            }
            else
            {
                var avg = (MatrixValue)AvgFunction.Average(M);
                var scale = 1.0;
                var s = new MatrixValue(1, M.DimensionX);

                for (var i = 1; i <= M.DimensionY; i++)
                    for (int j = 1; j <= M.DimensionX; j++)
                        s[1, j] += (M[i, j] - avg[j]).Square();

                scale /= M.DimensionY;

                for (var i = 1; i <= s.DimensionY; i++)
                    for (int j = 1; j <= s.DimensionX; j++)
                        s[i, j] *= scale;

                return s;
            }
        }
    }
}