using System;

namespace YAMP
{
	[Description("Sorts the columns of a given matrix or the complete vector.")]
	[Kind(PopularKinds.Function)]
    sealed class SortFunction : ArgumentFunction
	{
		[Description("Gives back the sorted columns of the passed matrix. The sorting is done from the lowest to the highest number, i.e. in ascending order.")]
		[Example("sort([1, 2, 0, 9, 5])", "Sorts the vector [1, 2, 0, 9, 5], resulting in [0, 1, 2, 5, 9].")]
		[Example("sort([1, 2, 0, 9, 5; 10, 8, 4, 6, 1])", "Sorts the matrix [1, 2, 0, 9, 5; 10, 8, 4, 6, 1], resulting in [0, 1, 2, 5, 9; 1, 4, 6, 8, 10].")]
		public MatrixValue Function(MatrixValue M)
		{
			if (M.IsVector)
				return M.VectorSort();

			var result = new MatrixValue();

			for(var i = 0; i < M.DimensionY; i++)
			{
				var vec = M.GetSubMatrix(i, i + 1, 0, M.DimensionX);
                vec = vec.VectorSort();
				result = result.AddRow(vec);
			}

			return result;
		}
	}
}
