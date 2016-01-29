using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Computes the eigenvalues and eigenvectors of a given matrix.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Eigendecomposition_of_a_matrix")]
    sealed class EigFunction : ArgumentFunction
	{
		[Description("Solves the eigenproblem of a matrix A and return a vector with all (and degenerate) eigenvalues.")]
        [Example("eig([1,2,3;4,5,6;7,8,9])", "Returns a vector with the three eigenvalues 16.11684, -1.11684 and 0 of this 3x3 matrix.")]
        [Example("[vals, vecs] = eig([1,2,3;4,5,6;7,8,9])", "Saves a vector with the three eigenvalues 16.11684, -1.11684 and 0 of this 3x3 matrix in the variable vals and a matrix containing the eigenvectors in the variable vecs.")]
		[Returns(typeof(MatrixValue), "The eigenvalues of the matrix stored in a vector.", 0)]
        [Returns(typeof(MatrixValue), "The eigenvectors of the matrix stored in a matrix.", 1)]
		public ArgumentsValue Function(MatrixValue M)
		{
			var ev = new Eigenvalues(M as MatrixValue);
			var m = new MatrixValue(ev.RealEigenvalues.Length, 1);
			var n = ev.GetV();

			for (var i = 1; i <= ev.RealEigenvalues.Length; i++)
				m[i, 1] = new ScalarValue(ev.RealEigenvalues[i - 1], ev.ImagEigenvalues[i - 1]);

			return new ArgumentsValue(m, n);
		}
	}
}
