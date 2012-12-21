using System;

namespace YAMP
{
    [Kind(PopularKinds.Function)]
    [Description("Calculates the arithmetic mean, which is the sum of n elements divided by n.")]
    class AvgFunction : ArgumentFunction
    {
        [Description("The arithmetic mean is the naive way of averaging over a number of items. Here the arithmetic mean is taken, i.e. the sum over all items divided by the number of items.")]
        [Example("avg([1, 4, 8])", "Computes the arithmetic mean of 1, 4 and 8, i.e. (1 + 4 + 8) / 3. The result is 13 / 3 or 4.333.")]
        public ScalarValue Function(MatrixValue M)
        {
            if (M.Length == 0)
                return new ScalarValue();

            var s = new ScalarValue();

            for (var i = 1; i <= M.Length; i++)
                s += M[i];

            return s / M.Length;
        }
    }
}
