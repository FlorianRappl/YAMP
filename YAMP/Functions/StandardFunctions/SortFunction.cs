using System;

namespace YAMP
{
	[Description("Sorts the columns of a given matrix or the complete vector.")]
	[Kind(PopularKinds.Function)]
	class SortFunction : ArgumentFunction
	{
		[Description("Gives back the sorted columns of the passed matrix. The sorting is done from the lowest to the highest number, i.e. in ascending order.")]
		[Example("sort([1, 2, 0, 9, 5])", "Sorts the vector [1, 2, 0, 9, 5], resulting in [0, 1, 2, 5, 9].")]
		[Example("sort([1, 2, 0, 9, 5; 10, 8, 4, 6, 1])", "Sorts the matrix [1, 2, 0, 9, 5; 10, 8, 4, 6, 1], resulting in [0, 1, 2, 5, 9; 1, 4, 6, 8, 10].")]
		public MatrixValue Function(MatrixValue matrix)
		{
			if (matrix.IsVector)
				return matrix.VectorSort();

			var result = new MatrixValue();

			for(var i = 0; i < matrix.DimensionX; i++)
			{
				var vec = matrix.SubMatrix(0, matrix.DimensionY, i, i + 1);
				result = result.AddRow(vec.VectorSort());
			}

			return result.Transpose();
		}
	}
}
