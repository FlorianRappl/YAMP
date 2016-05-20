namespace YAMP
{
	[Description("SumFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class SumFunction : ArgumentFunction
    {
        [Description("SumFunctionDescriptionForScalar")]
        public ScalarValue Function(ScalarValue x)
		{
            return x;
		}

        [Description("SumFunctionDescriptionForMatrix")]
        [Example("sum([1,2,3,4,5,6,7,-1])", "SumFunctionExampleForMatrix1")]
        [Example("sum([1,2;3,4;5,6;7,-1])", "SumFunctionExampleForMatrix2")]
        public Value Function(MatrixValue m)
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
                var M = new MatrixValue(1, m.DimensionX);

                for (var i = 1; i <= m.DimensionX; i++)
                {
                    M[1, i] = GetVectorSum(m.GetColumnVector(i));
                }

                return M;
            }
        }
		
		static ScalarValue GetVectorSum(MatrixValue vec)
		{
			var sum = ScalarValue.Zero;

            for (var i = 1; i <= vec.Length; i++)
            {
                sum = sum + vec[i];
            }
			
			return sum;
		}
	}
}

