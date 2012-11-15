using System;

namespace YAMP
{
	[Description("Generates an identity matrix.")]
	[Kind(PopularKinds.Function)]
    class EyeFunction : ArgumentFunction
    {
        [Description("Generates the 1x1 matrix.")]
        public MatrixValue Function()
        {
            return MatrixValue.One(1);
        }

        [Description("Generates an n-dimensional identity matrix.")]
        [Example("eye(5)", "Returns a 5x5 matrix with 1 on the diagonal and 0 else.")]
        public MatrixValue Function(ScalarValue dim)
        {
            var k = (int)dim.Value;
            return MatrixValue.One(k);
        }
    }
}
