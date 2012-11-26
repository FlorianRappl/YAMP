using System;
using YAMP.Numerics;

namespace YAMP
{
	[Kind(PopularKinds.Function)]
	[Description("In linear algebra, the singular value decomposition (SVD) is a factorization of a real or complex matrix, with many useful applications in signal processing and statistics.")]
	class SvdFunction : ArgumentFunction
	{
		[Description("Applications which employ the SVD include computing the pseudoinverse, least squares fitting of data, matrix approximation, and determining the rank, range and null space of a matrix.")]
		[Example("svd([1, 0, 0, 0, 2; 0, 0, 3, 0, 0; 0, 0, 0, 0, 0; 0, 4, 0, 0, 0])", "Computes the matrices Sigma (singular values), U (left-singular vectors) and V* (right-singular vectors) of the matrix.")]
		[Example("[S, U, V] = svd([1, 0, 0, 0, 2; 0, 0, 3, 0, 0; 0, 0, 0, 0, 0; 0, 4, 0, 0, 0])", "Computes the matrices Sigma, U and V* and stores them in the matrices S, U, V.")]
		[Returns(typeof(MatrixValue), "The diagonal matrix of singular values (Sigma).")]
        [Returns(typeof(MatrixValue), "The left singular vectors (U).")]
        [Returns(typeof(MatrixValue), "The right singular vectors (V*).")]
		public ArgumentsValue Function(MatrixValue m)
		{
			var svd = new SingularValueDecomposition(m);
			return new ArgumentsValue(svd.S, svd.GetU(), svd.GetV());
		}
	}
}
