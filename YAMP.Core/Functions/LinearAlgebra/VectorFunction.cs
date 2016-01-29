using System;

namespace YAMP
{
    [Description("Creates a vector (a n-times-1 matrix) out of the given data.")]
    [Kind(PopularKinds.Function)]
    sealed class VectorFunction : ArgumentFunction
    {
        [Description("Creates a vector with the length of the given matrix. An mxn-matrix will be transformed to a vector with length m*n.")]
        [Example("vector([1,2;3,4])", "Creates the vector [1, 2, 3, 4] out of the given 2x2 matrix.")]
        public MatrixValue Function(MatrixValue M)
        {
            var v = new MatrixValue(M.Length, 1);
            var k = 1;

            for (var i = 1; i <= M.DimensionX; i++)
                for (var j = 1; j <= M.DimensionY; j++)
                    v[k++] = M[j, i];

            return v;
        }
    }
}
