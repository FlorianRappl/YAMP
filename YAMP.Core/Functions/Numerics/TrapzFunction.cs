namespace YAMP
{
    using YAMP.Numerics;

    [Description("TrapzFunctionDescription")]
	[Kind(PopularKinds.Function)]
    [Link("TrapzFunctionLink")]
    sealed class TrapzFunction : ArgumentFunction
    {
        [Description("TrapzFunctionDescriptionForMatrixMatrix")]
        [Example("trapz(sin(0:0.1:Pi), 0:0.1:Pi)", "TrapzFunctionExampleForMatrixMatrix1")]
        public ScalarValue Function(MatrixValue y, MatrixValue x)
        {
            var integral = new TrapezIntegrator(y);
            return integral.Integrate(x);
        }
    }
}
