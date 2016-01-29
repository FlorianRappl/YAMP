using System;

namespace YAMP
{
    [Kind(PopularKinds.Statistic)]
    [Description("Calculates the arithmetic mean, which is the sum of n elements divided by n.")]
    [Link("http://en.wikipedia.org/wiki/Arithmetic_mean")]
    sealed class AvgFunction : ArgumentFunction
    {
        [Description("The arithmetic mean is the naive way of averaging over a number of items. Here the arithmetic mean is taken, i.e. the sum over all items divided by the number of items.")]
        [Example("avg([1, 4, 8])", "Computes the arithmetic mean of 1, 4 and 8, i.e. (1 + 4 + 8) / 3. The result is 13 / 3 or 4.333.")]
        [Example("avg([1, 2, 3; 2, 3, 2])", "Computes the arithmetic mean over the columns, i.e. the result is a row [1.5,2.5,2.5].")]
        public Value Function(MatrixValue M)
        {
            return YMath.Average(M);
        }
    }
}
