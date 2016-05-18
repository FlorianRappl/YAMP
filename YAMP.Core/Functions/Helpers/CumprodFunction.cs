namespace YAMP
{
    [Description("CumprodFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class CumprodFunction : ArgumentFunction
    {
        [Description("CumprodFunctionDescriptionForScalar")]
        public ScalarValue Function(ScalarValue x)
        {
            return x;
        }

        [Description("CumprodFunctionDescriptionForMatrix")]
        [Example("cumprod([1, 2, 3, 0, 3, 2])", "CumprodFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue m)
        {
            if (m.DimensionX == 1)
            {
                return GetVectorProd(m.GetColumnVector(1));
            }
            else if (m.DimensionY == 1)
            {
                return GetVectorProd(m.GetRowVector(1));
            }
            else
            {
                var M = new MatrixValue(m.DimensionY, m.DimensionX);

                for (var i = 1; i <= m.DimensionX; i++)
                {
                    M.SetColumnVector(i, GetVectorProd(m.GetColumnVector(i)));
                }

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
