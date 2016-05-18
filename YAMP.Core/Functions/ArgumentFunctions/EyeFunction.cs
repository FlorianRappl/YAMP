using System;

namespace YAMP
{
    [Description("EyeFunctionDescription")]
	[Kind(PopularKinds.Function)]
    [Link("EyeFunctionLink")]
    sealed class EyeFunction : ArgumentFunction
    {
        [Description("EyeFunctionDescriptionForVoid")]
        public MatrixValue Function()
        {
            return MatrixValue.One(1);
        }

        [Description("EyeFunctionDescriptionForScalar")]
        [Example("eye(5)", "EyeFunctionExampleForScalar1")]
        public MatrixValue Function(ScalarValue dim)
        {
            var n = dim.GetIntegerOrThrowException("dim", Name);
            return MatrixValue.One(n);
        }

        [Description("EyeFunctionDescriptionForScalarScalar")]
        [Example("eye(5, 3)", "EyeFunctionExampleForScalarScalar1")]
        public MatrixValue Function(ScalarValue n, ScalarValue m)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);
            var nm = m.GetIntegerOrThrowException("m", Name);
            return MatrixValue.One(nn, nm);
        }
    }
}
