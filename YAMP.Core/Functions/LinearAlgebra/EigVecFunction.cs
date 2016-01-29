using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Computes the eigenvectors of a given matrix.")]
    [Kind(PopularKinds.Function)]
    sealed class EigVecFunction : ArgumentFunction
    {
        [Description("Solves the eigenproblem of a matrix A and return a matrix with all (and degenerate) eigenvectors.")]
        [Example("eigvec([1,2,3;4,5,6;7,8,9])", "Returns a 3x3 matrix with the three eigenvectors of this 3x3 matrix.")]
        public MatrixValue Function(MatrixValue M)
        {
            var ev = new Eigenvalues(M as MatrixValue);
            return ev.GetV();
        }
    }
}
