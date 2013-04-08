using System;
using YAMP.Numerics;

namespace YAMP
{
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/LU_decomposition")]
	[Description("In linear algebra, LU decomposition (also called LU factorization) factorizes a matrix as the product of a lower triangular matrix and an upper triangular matrix. The product sometimes includes a permutation matrix as well. The LU decomposition can be viewed as the matrix form of Gaussian elimination.")]
    sealed class LUFunction : ArgumentFunction
	{
		[Description("An LU decomposition is a decomposition of the form PA = LU, where L is a lower triangular matrix, P is a permutation matrix containing the pivot elements and U is an upper triangular matrix.")]
		[Example("lu([4, 3; 6, 3])", "Computes the LU-decomposition of the matrix and returns the lower matrix L.")]
		[Example("[L, U, P] = lu([4, 3; 6, 3])", "Computes the LU-decomposition of the matrix and returns the lower matrix L (saved in the variable L) and the upper matrix R (saved in the variable R). The permutation matrix is saved in the variable P.")]
		[Returns(typeof(MatrixValue), "The lower matrix L.", 0)]
		[Returns(typeof(MatrixValue), "The upper (right) matrix U.", 1)]
		[Returns(typeof(MatrixValue), "The permutation matrix P.", 2)]
		public ArgumentsValue Function(MatrixValue M)
		{
			var lu = new LUDecomposition(M);
			return new ArgumentsValue(lu.L, lu.U, lu.Pivot);
		}
	}
}
