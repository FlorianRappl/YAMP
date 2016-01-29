using System;

namespace YAMP
{
	[Description("Generates a matrix with only zeros.")]
	[Kind(PopularKinds.Function)]
    sealed class ZerosFunction : ArgumentFunction
    {
        [Description("Generates a 1x1 matrix.")]
        public MatrixValue Function()
        {
            return new MatrixValue(1,1);
        }

        [Description("Generates an n-dimensional matrix with only zeros.")]
        [Example("zeros(5)", "Returns a 5x5 matrix with 0 in each cell.")]
        public MatrixValue Function(ScalarValue dim)
        {
            var k = (int)dim.Re;
            return new MatrixValue(k, k);
        }

        [Description("Generates a n-by-m matrix with only zeros.")]
        [Example("zeros(5,2)", "Returns a 5x2 matrix with 0 in each cell.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            var k = (int)rows.Re;
            var l = (int)cols.Re;
            return new MatrixValue(k, l);
        }
    }
}
