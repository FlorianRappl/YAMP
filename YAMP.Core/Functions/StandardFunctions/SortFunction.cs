namespace YAMP
{
	[Description("SortFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class SortFunction : ArgumentFunction
	{
		[Description("SortFunctionDescriptionForMatrix")]
		[Example("sort([1, 2, 0, 9, 5])", "SortFunctionExampleForMatrix1")]
		[Example("sort([1, 2, 0, 9, 5; 10, 8, 4, 6, 1])", "SortFunctionExampleForMatrix2")]
		public MatrixValue Function(MatrixValue M)
		{
			if (!M.IsVector)
            {
                var result = new MatrixValue();

                for (var i = 0; i < M.DimensionY; i++)
                {
                    var vec = M.GetSubMatrix(i, i + 1, 0, M.DimensionX);
                    vec = vec.VectorSort();
                    result = result.AddRow(vec);
                }

                return result;
            }

			return M.VectorSort();
		}
	}
}
