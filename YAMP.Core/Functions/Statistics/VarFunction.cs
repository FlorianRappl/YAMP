using System;

namespace YAMP
{
    [Description("In probability theory and statistics, the variance is a measure of how far a set of numbers is spread out.")]
    [Kind(PopularKinds.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Variance")]
    sealed class VarFunction : ArgumentFunction
    {
        [Description("The variance is obtained by calculating the data set consisting of each data point in the original data set subtracting the arithmetic mean for the data set, and then squaring it.")]
        [Example("var([1, 2, 3, 4, 5, 6])", "Gives the variance for throwing a perfect sided die, which is roughly 2.9.")]
        [Example("var([1, 2, 3; 4, 5, 6; 7, 8, 9])", "Computes the variance for each column of the given matrix.")]
        public Value Function(MatrixValue M)
        {
            return YMath.Variance(M);
        }
    }
}