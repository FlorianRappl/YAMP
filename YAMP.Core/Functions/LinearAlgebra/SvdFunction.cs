using System;
using YAMP.Numerics;

namespace YAMP
{
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Singular_value_decomposition")]
	[Description("In linear algebra, the singular value decomposition (SVD) is a factorization of a real or complex matrix, with many useful applications in signal processing and statistics.")]
    sealed class SvdFunction : ArgumentFunction
	{
		[Description("Applications which employ the SVD include computing the pseudoinverse, least squares fitting of data, matrix approximation, and determining the rank, range and null space of a matrix.")]
		[Example("svd([1, 0, 0, 0, 2; 0, 0, 3, 0, 0; 0, 0, 0, 0, 0; 0, 4, 0, 0, 0])", "Computes the matrices Sigma (singular values), U (left-singular vectors) and V* (right-singular vectors) of the matrix.")]
		[Example("[S, U, V] = svd([1, 0, 0, 0, 2; 0, 0, 3, 0, 0; 0, 0, 0, 0, 0; 0, 4, 0, 0, 0])", "Computes the matrices Sigma, U and V* and stores them in the matrices S, U, V.")]
		[Returns(typeof(MatrixValue), "The diagonal matrix of singular values (Sigma).", 0)]
        [Returns(typeof(MatrixValue), "The left singular vectors (U).", 1)]
        [Returns(typeof(MatrixValue), "The right singular vectors (V*).", 2)]
		public ArgumentsValue Function(MatrixValue M)
		{
			var svd = new SingularValueDecomposition(M);
			return new ArgumentsValue(svd.S, svd.GetU(), svd.GetV());
		}
	}
}
