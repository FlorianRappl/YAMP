using System;
using YAMP.Numerics;

namespace YAMP
{
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/QR_decomposition")]
    [Description("In linear algebra, the Cholesky decomposition or Cholesky triangle is a decomposition of a Hermitian, positive-definite matrix into the product of a lower triangular matrix and its conjugate transpose.")]
    sealed class CholFunction : ArgumentFunction
	{
		[Description("Computes the Cholesky decomposition of a given SPD matrix and returns the L matrix which is given by L * L' = M, where L' is the adjungate matrix of L.")]
		[Example("chol([4, 12, -16; 12, 37, -43; -16, -43, 98])", "Computes the matrix L of the given matrix.")]
		public MatrixValue Function(MatrixValue M)
		{
			var chol = new CholeskyDecomposition(M);
            return chol.GetL();
		}
	}
}
