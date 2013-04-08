using System;

namespace YAMP
{
    [Kind(PopularKinds.Statistic)]
    [Description("Calculates the harmonic mean, which is the number of elements divided by the sum of the inverse of each element. The harmonic mean is the special case of the power mean. As it tends strongly toward the least elements of the list, it may (compared to the arithmetic mean) mitigate the influence of large outliers and increase the influence of small values.")]
    [Link("http://en.wikipedia.org/wiki/Harmonic_mean")]
    sealed class HmeanFunction : ArgumentFunction
    {
        [Description("The harmonic mean (sometimes called the subcontrary mean) is one of several kinds of average. Typically, it is appropriate for situations when the average of rates is desired.")]
        [Example("hmean([1, 4, 8])", "Computes the harmonic mean of 1, 4 and 8, i.e. 3 / (1 + 0.25 + 0.125). The result is 24/11 or 2.1818.")]
        [Example("hmean([1, 4, 8; 2, 5, 7])", "Computes the harmonic mean of 1, 4 and 8, as well as 2, 5 and 7. The result is a 1x2 matrix.")]
        public Value Function(MatrixValue M)
        {
            return YMath.HarmonicMean(M);
        }
    }
}
