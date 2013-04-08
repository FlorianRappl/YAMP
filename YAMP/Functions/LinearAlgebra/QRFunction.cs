using System;
using YAMP.Numerics;

namespace YAMP
{
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/QR_decomposition")]
	[Description("In linear algebra, a QR decomposition (also called a QR factorization) of a matrix is a decomposition of a matrix A into a product A=QR of an orthogonal matrix Q and an upper triangular matrix R. QR decomposition is often used to solve the linear least squares problem, and is the basis for a particular eigenvalue algorithm, the QR algorithm.")]
    sealed class QRFunction : ArgumentFunction
	{
		[Description("Any real square matrix A may be decomposed as A = QR, where Q is an orthogonal matrix (its columns are orthogonal unit vectors) and R is an upper triangular matrix (also called right triangular matrix). This generalizes to a complex square matrix A and a unitary matrix Q. If A is invertible, then the factorization is unique if we require that the diagonal elements of R are positive.")]
		[Example("qr([12, -51, 4; 6, 167, -68; -4, 24, -41])", "Computes the matrix Q of the given matrix.")]
		[Example("[Q, R] = qr([12, -51, 4; 6, 167, -68; -4, 24, -41])", "Computes the matrices Q and R and stores them in the variables Q and R.")]
		[Returns(typeof(MatrixValue), "The economy sized orthogonal factor matrix Q.", 0)]
		[Returns(typeof(MatrixValue), "The upper triangular factor matrix R.", 1)]
		public ArgumentsValue Function(MatrixValue M)
		{
			var qr = QRDecomposition.Create(M);
			return new ArgumentsValue(qr.Q, qr.R);
		}
	}
}
