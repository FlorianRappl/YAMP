namespace YAMP
{
    using System;

    [Description("ZerosFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class ZerosFunction : ArgumentFunction
    {
        [Description("ZerosFunctionDescriptionForVoid")]
        public MatrixValue Function()
        {
            return new MatrixValue(1,1);
        }

        [Description("ZerosFunctionDescriptionForScalar")]
        [Example("zeros(5)", "ZerosFunctionExampleForScalar1")]
        public MatrixValue Function(ScalarValue dim)
        {
            var k = (Int32)dim.Re;
            return new MatrixValue(k, k);
        }

        [Description("ZerosFunctionDescriptionForScalarScalar")]
        [Example("zeros(5,2)", "ZerosFunctionExampleForScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            var k = (Int32)rows.Re;
            var l = (Int32)cols.Re;
            return new MatrixValue(k, l);
        }
    }
}
