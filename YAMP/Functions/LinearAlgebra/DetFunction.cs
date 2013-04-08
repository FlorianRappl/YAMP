using System;

namespace YAMP
{
	[Description("Calculates the determinant of the given matrix.")]
	[Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Determinant")]
    sealed class DetFunction : ArgumentFunction
	{
		[Description("Uses the best algorithm to compute the determinant.")]
		[Example("det([1,3;-1,0])", "Computes the determinant of the matrix [1,3;-1,0]; returns 3.")]
		public ScalarValue Function(MatrixValue M)
		{
			return M.Det();
		}

		[Description("Returns the argument.")]
		[Example("det(5)", "The determinant of a 1x1 matrix is the argument itself.")]
		public ScalarValue Function(ScalarValue x)
		{
			return x;
		}
	}
}

