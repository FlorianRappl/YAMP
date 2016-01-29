using System;

namespace YAMP
{
    [Description("Computes the cumulative sum of the given arguments.")]
    [Kind(PopularKinds.Function)]
    sealed class CumsumFunction : ArgumentFunction
    {
        [Description("Just returns the given scalar, since the cumulative sum of one scalar is the scalar itself.")]
        public ScalarValue Function(ScalarValue x)
        {
            return x;
        }

        [Description("Computes the cumulative sum of a vector or a list of vectors, i.e. a matrix.")]
        [Example("cumsum([1, 2, 3, 0, 3, 2])", "Returns the vector [1, 3, 6, 6, 9, 11], which is the cumulative sum of the given vector.")]
        public MatrixValue Function(MatrixValue m)
        {
            if (m.DimensionX == 1)
                return GetVectorSum(m.GetColumnVector(1));
            else if (m.DimensionY == 1)
                return GetVectorSum(m.GetRowVector(1));
            else
            {
                var M = new MatrixValue(m.DimensionY, m.DimensionX);

                for (var i = 1; i <= m.DimensionX; i++)
                    M.SetColumnVector(i, GetVectorSum(m.GetColumnVector(i)));

                return M;
            }
        }

        MatrixValue GetVectorSum(MatrixValue vec)
        {
            var m = new MatrixValue(vec.DimensionY, vec.DimensionX);
            var sum = ScalarValue.Zero;

            for (var i = 1; i <= vec.Length; i++)
            {
                sum += vec[i];
                m[i] = sum;
            }

            return m;
        }
    }
}
