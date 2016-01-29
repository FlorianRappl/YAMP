using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("The function computes the upper triangle matrix of a given matrix.")]
    [Kind(PopularKinds.Function)]
    sealed class TriuFunction : ArgumentFunction
    {
        [Description("Given a square matrix the function computes the upper triangular matrix.")]
        [Example("triu(rand(4))", "Computes the upper triangle matrix of the given 4x4 random matrix.")]
        public MatrixValue Function(MatrixValue M)
        {
            var lu = new LUDecomposition(M);
            return lu.U;
        }
    }
}
