namespace YAMP
{
    [Description("CumsumFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class CumsumFunction : ArgumentFunction
    {
        [Description("CumsumFunctionDescriptionForScalar")]
        public ScalarValue Function(ScalarValue x)
        {
            return x;
        }

        [Description("CumsumFunctionDescriptionForMatrix")]
        [Example("cumsum([1, 2, 3, 0, 3, 2])", "CumsumFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue m)
        {
            if (m.DimensionX == 1)
            {
                return GetVectorSum(m.GetColumnVector(1));
            }
            else if (m.DimensionY == 1)
            {
                return GetVectorSum(m.GetRowVector(1));
            }
            else
            {
                var M = new MatrixValue(m.DimensionY, m.DimensionX);

                for (var i = 1; i <= m.DimensionX; i++)
                {
                    M.SetColumnVector(i, GetVectorSum(m.GetColumnVector(i)));
                }

                return M;
            }
        }

        static MatrixValue GetVectorSum(MatrixValue vec)
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
