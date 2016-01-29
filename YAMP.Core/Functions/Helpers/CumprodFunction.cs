using System;

namespace YAMP
{
    [Description("Computes the cumulative product of the given arguments.")]
    [Kind(PopularKinds.Function)]
    sealed class CumprodFunction : ArgumentFunction
    {
        [Description("Just returns the given scalar, since the cumulative product of one scalar is the scalar itself.")]
        public ScalarValue Function(ScalarValue x)
        {
            return x;
        }

        [Description("Computes the cumulative product of a vector or a list of vectors, i.e. a matrix.")]
        [Example("cumprod([1, 2, 3, 0, 3, 2])", "Returns the vector [1, 2, 6, 0, 0, 0], which is the cumulative product of the given vector.")]
        public MatrixValue Function(MatrixValue m)
        {
            if (m.DimensionX == 1)
                return GetVectorProd(m.GetColumnVector(1));
            else if (m.DimensionY == 1)
                return GetVectorProd(m.GetRowVector(1));
            else
            {
                var M = new MatrixValue(m.DimensionY, m.DimensionX);

                for (var i = 1; i <= m.DimensionX; i++)
                    M.SetColumnVector(i, GetVectorProd(m.GetColumnVector(i)));

                return M;
            }
        }

        MatrixValue GetVectorProd(MatrixValue vec)
        {
            var m = new MatrixValue(vec.DimensionY, vec.DimensionX);
            var prod = ScalarValue.One;

            for (var i = 1; i <= vec.Length; i++)
            {
                prod *= vec[i];
                m[i] = prod;
            }

            return m;
        }
    }
}
