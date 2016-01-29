using System;

namespace YAMP
{
    [Description("In probability theory and statistics, auto-correlation is a measure of how much one random variables is correlated with itself at different offsets.")]
    [Kind(PopularKinds.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Autocorrelation")]
    sealed class ACorFunction : ArgumentFunction
    {
        [Description("This function returns a vector with auto-correlations for different offsets. All matrices are treated as vectors.")]
        [Example("acor(3 + randn(100, 1))", "Gives the auto-correlation for a normal dirstributed random variables of variance 1 and different offsets.")]
        public MatrixValue Function(MatrixValue M)
        {
            if (M.Length <= 1)
            {
                return new MatrixValue();
            }
            else
            {
                int nOffset = (int)(10 * Math.Log10(M.Length));
                if (nOffset < 0)
                    nOffset = 0;
                else if (nOffset >= M.Length)
                    nOffset = M.Length - 1;
                return YMath.CrossCorrelation(M, M, nOffset);
            }
        }

        [Example("acor(3+randn(100,1), 4)", "Gives the first 4 auto-correlations for a normal dirstributed random variables of variance 1.")]
        public MatrixValue Function(MatrixValue M, ScalarValue nLag)
        {
            if (M.Length <= 1)
                return new MatrixValue();

            var nOffset = nLag.GetIntegerOrThrowException("nLag", Name);

            if (nOffset < 0)
                nOffset = 0;
            else if (nOffset >= M.Length)
                nOffset = M.Length - 1;

            return YMath.CrossCorrelation(M, M, nOffset);
        }
    }
}