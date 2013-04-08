using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("In mathematics, the beta function, also called the Euler integral of the first kind, is a special function. The beta function was studied by Euler and Legendre and was given its name by Jacques Binet; its symbol Β is a Greek capital β rather than the similar Latin capital B.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Beta_function")]
    sealed class BetaFunction : ArgumentFunction
    {
        [Description("Computes the beta function for corresponding values z and w. The values must be real and positive.")]
        [Example("beta(5, 3)", "Evaluates the beta function at z = 5 and w = 3.")]
        public ScalarValue Function(ScalarValue z, ScalarValue w)
        {
            return new ScalarValue(Gamma.Beta(z.Re, w.Re));
        }

        [Description("Computes the beta function for corresponding elements of arrays Z and W. The arrays must be real and positive. They must be the same size, or either can be scalar.")]
        [Example("beta(1:5, 3:7)", "Evaluates the beta function at the points (1, 0), (2, 1), ... up to (5, 4).")]
        public MatrixValue Function(MatrixValue Z, MatrixValue W)
        {
            if (Z.DimensionX != W.DimensionX || Z.DimensionY != W.DimensionY)
                throw new YAMPDifferentDimensionsException(Z.DimensionY, Z.DimensionX, W.DimensionY, W.DimensionX);

            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var i = 1; i <= Z.DimensionX; i++)
                for (var j = 1; j <= Z.DimensionY; j++)
                    M[j, i] = Function(Z[j, i], W[j, i]);

            return M;
        }

        [Description("Computes the beta function for corresponding elements of an array Z and a scalar value w. The values must be real and positive.")]
        [Example("beta(1:10, 3)", "Evaluates the beta function at w = 3 for z = 1 to 10.")]
        public MatrixValue Function(MatrixValue Z, ScalarValue w)
        {
            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var i = 1; i <= Z.DimensionX; i++)
                for (var j = 1; j <= Z.DimensionY; j++)
                    M[j, i] = Function(Z[j, i], w);

            return M;
        }

        [Description("Computes the beta function for corresponding elements of a scalar value z and an arrays W. The values must be real and positive.")]
        [Example("beta(2, 1:10)", "Evaluates the beta function at z = 2 for w = 1 to 10.")]
        public MatrixValue Function(ScalarValue z, MatrixValue W)
        {
            return Function(W, z);
        }
    }
}
