using System;
using YAMP.Numerics;

namespace YAMP
{
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/QR_decomposition")]
    [Description("In linear algebra, the Cholesky decomposition or Cholesky triangle is a decomposition of a Hermitian, positive-definite matrix into the product of a lower triangular matrix and its conjugate transpose.")]
    class CholeskyFunction : ArgumentFunction
	{
		[Description("Computes the Cholesky decomposition of a given matrix and returns the L matrix which is given by L * L^t = M, where L^t is the adjungate matrix of L.")]
		[Example("cholesky([4, 12, 16; 12, 37, -43; -16, -43, 98])", "Computes the matrix L of the given matrix.")]
		public MatrixValue Function(MatrixValue M)
		{
			var chol = new CholeskyDecomposition(M);
            return chol.GetL();
		}
	}
}
