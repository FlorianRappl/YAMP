using System;

namespace YAMP
{
	[Description("Inverts the given matrix.")]
	[Kind(PopularKinds.Function)]
    sealed class InvFunction : ArgumentFunction
	{
		[Description("Finds the inverse of a given number.")]
		[Example("inv(5)", "Inverts the number 5, resulting in 0.2.")]
		public ScalarValue Function(ScalarValue x)
		{
			return 1.0 / x;
		}

		[Description("Tries to find the inverse of the matrix, i.e. inv(A)=A^-1.")]
		[Example("inv([0,2;1,0])", "Inverts the matrix [0,2;1,0], resulting in [0,1;0.5,0].")]
		public MatrixValue Function(MatrixValue M)
		{
			return M.Inverse();
		}
	}
}

