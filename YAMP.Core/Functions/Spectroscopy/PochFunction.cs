using System;
using YAMP.Numerics;


namespace YAMP.Functions
{
    [Description("In mathematics, the Pochhammer symbol introduced by Leo August Pochhammer is the notation (x)n, where n is a non-negative integer. Depending on the context the Pochhammer symbol may represent either the rising factorial or the falling factorial.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Pochhammer_symbol")]
    sealed class PochFunction : ArgumentFunction
    {
        [Description("Computes the Pochhammer symbol using the (in general complex) values z and n, where poch(z, n) = Gamma(z + n) / Gamma(z).")]
        [Example("poch(1, 0.5)", "Evaluates gamma(1.5) / gamma(1).")]
        public ScalarValue Function(ScalarValue z, ScalarValue n)
        {
            return Gamma.LinearGamma(z + n) / Gamma.LinearGamma(z);
        }

        [Description("Computes the Pochhammer symbol using the (in general complex) matrix Z and the scalar n, where poch(Z, n)(i, j) = Gamma(Z(i, j) + n) / Gamma(Z(i, j)).")]
        [Example("poch([1, 2, 3, 4, 5, 6], 0.5)", "Evaluates gamma(1.5) / gamma(1), gamma(2.5) / gamma(2), ... and returns the matrix containing the values.")]
        public MatrixValue Function(MatrixValue Z, ScalarValue n)
        {
            var N = new MatrixValue(Z.DimensionY, Z.DimensionX, n);
            return Function(Z, N);
        }

        [Description("Computes the Pochhammer symbol using the (in general complex) scalar z and the matrix N, where poch(z, N)(i, j) = Gamma(z + N(i, j)) / Gamma(z).")]
        [Example("poch(1, [0.1, 0.3, 0.5, 0.7])", "Evaluates gamma(1.1) / gamma(1), gamma(1.3) / gamma(1), ... and returns the matrix containing the values.")]
        public MatrixValue Function(ScalarValue z, MatrixValue N)
        {
            var Z = new MatrixValue(N.DimensionY, N.DimensionX, z);
            return Function(Z, N);
        }

        [Description("Computes the Pochhammer symbol using the (in general complex) matrices Z and N, where poch(Z, N)(i, j) = Gamma(Z(i, j) + N(i, j)) / Gamma(Z(i, j)). The matrices Z and N must have equal dimensions.")]
        [Example("poch([1, 2, 3, 4, 5, 6], [0.1, 0.3, 0.5, 0.7, 0.9, 1.1])", "Evaluates gamma(1.1) / gamma(1), gamma(2.3) / gamma(2), gamma(3.5) / gamma(3), ... and returns the matrix containing the values.")]
        public MatrixValue Function(MatrixValue Z, MatrixValue N)
        {
            if (Z.DimensionY != N.DimensionY || Z.DimensionX != N.DimensionX)
                throw new YAMPDifferentDimensionsException(Z, N);

            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for(var i = 1; i <= M.DimensionX; i++)
                for (var j = 1; j <= M.DimensionY; j++)
                    M[j, i] = Gamma.LinearGamma(Z[j, i] + N[j, i]) / Gamma.LinearGamma(Z[j, i]);

            return M;
        }
    }
}
