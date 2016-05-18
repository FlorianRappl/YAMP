namespace YAMP
{
    using System;

    [Description("OnesFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class OnesFunction : ArgumentFunction
    {
        [Description("OnesFunctionDescriptionForVoid")]
        public MatrixValue Function()
        {
            return MatrixValue.Ones(1, 1);
        }

        [Description("OnesFunctionDescriptionForScalar")]
        [Example("ones(5)", "OnesFunctionExampleForScalar1")]
        public MatrixValue Function(ScalarValue dim)
        {
            var k = (Int32)dim.Re;
            return MatrixValue.Ones(k, k);
        }

        [Description("OnesFunctionDescriptionForScalarScalar")]
        [Example("ones(5,2)", "OnesFunctionExampleForScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            var k = (Int32)rows.Re;
            var l = (Int32)cols.Re;
            return MatrixValue.Ones(k, l);
        }
    }
}
