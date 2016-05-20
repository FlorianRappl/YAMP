namespace YAMP
{
	[Description("ProdFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class ProdFunction : ArgumentFunction
    {
        [Description("ProdFunctionDescriptionForScalar")]
        public Value Function(ScalarValue x)
		{
            return x;
		}

        [Description("ProdFunctionDescriptionForMatrix")]
        [Example("prod([1,2,3,4,5,6,7,-1])", "ProdFunctionExampleForMatrix1")]
        [Example("prod([1,2;3,4;5,6;7,-1])", "ProdFunctionExampleForMatrix2")]
        public Value Function(MatrixValue m)
        {
            if (m.DimensionX == 1)
            {
                return GetVectorProduct(m.GetColumnVector(1));
            }
            else if (m.DimensionY == 1)
            {
                return GetVectorProduct(m.GetRowVector(1));
            }
            else
            {
                var M = new MatrixValue(1, m.DimensionX);

                for (var i = 1; i <= m.DimensionX; i++)
                {
                    M[1, i] = GetVectorProduct(m.GetColumnVector(i));
                }

                return M;
            }
        }
		
		static ScalarValue GetVectorProduct(MatrixValue vec)
		{
			var prod = ScalarValue.One;

            for (var i = 1; i <= vec.Length; i++)
            {
                prod = prod * vec[i];
            }
			
			return prod;
		}
	}
}

