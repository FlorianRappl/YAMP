using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("The function computes the lower triangle matrix of a given matrix.")]
    [Kind(PopularKinds.Function)]
    sealed class TrilFunction : ArgumentFunction
    {
        [Description("Given a square matrix the function computes the lower triangular matrix.")]
        [Example("tril(rand(4))", "Computes the lower triangle matrix of the given 4x4 random matrix, which is P * L from the P, L, U decomposition.")]
        public MatrixValue Function(MatrixValue M)
        {
            var lu = new LUDecomposition(M);
            return lu.Pivot * lu.L;
        }
    }
}
