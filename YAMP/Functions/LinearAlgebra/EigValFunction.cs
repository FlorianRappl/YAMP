using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Computes the eigenvalues of a given matrix.")]
	[Kind(PopularKinds.Function)]
    sealed class EigValFunction : ArgumentFunction
	{
		[Description("Solves the eigenproblem of a matrix A and return a vector with all (+degenerate) eigenvalues.")]
		[Example("eigval([1,2,3;4,5,6;7,8,9])", "Returns a vector with the three eigenvalues 16.11684, -1.11684 and 0 of this 3x3 matrix.")]
		public MatrixValue Function(MatrixValue M)
		{
			var ev = new Eigenvalues(M as MatrixValue);
			var m = new MatrixValue(ev.RealEigenvalues.Length, 1);

			for (var i = 1; i <= ev.RealEigenvalues.Length; i++)
				m[i, 1] = new ScalarValue(ev.RealEigenvalues[i - 1], ev.ImagEigenvalues[i - 1]);

			return m;
		}
	}
}
