using System;

namespace YAMP
{
    [Description("Generates an identity matrix. In linear algebra, the identity matrix or unit matrix of size n is the n x n square matrix with ones on the main diagonal and zeros elsewhere.")]
	[Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Identity_matrix")]
    sealed class EyeFunction : ArgumentFunction
    {
        [Description("Generates the 1x1 identity matrix, which is just 1.")]
        public MatrixValue Function()
        {
            return MatrixValue.One(1);
        }

        [Description("Generates an n-dimensional identity matrix.")]
        [Example("eye(5)", "Returns a 5x5 matrix with 1 on the diagonal and 0 else.")]
        public MatrixValue Function(ScalarValue dim)
        {
            var n = dim.GetIntegerOrThrowException("dim", Name);
            return MatrixValue.One(n);
        }

        [Description("Generates an n x m-dimensional identity matrix.")]
        [Example("eye(5, 3)", "Returns a 5x3 matrix with 1 on the diagonal and 0 else.")]
        public MatrixValue Function(ScalarValue n, ScalarValue m)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);
            var nm = m.GetIntegerOrThrowException("m", Name);
            return MatrixValue.One(nn, nm);
        }
    }
}
