namespace YAMP
{
    using YAMP.Numerics;

    [Description("SimpsFunctionDescription")]
	[Kind(PopularKinds.Function)]
    [Link("SimpsFunctionLink")]
    sealed class SimpsFunction : ArgumentFunction
    {
        [Description("SimpsFunctionDescriptionForMatrixMatrix")]
        [Example("simps(sin(0:0.1:Pi), 0:0.1:Pi)", "SimpsFunctionExampleForMatrixMatrix1")]
        public ScalarValue Function(MatrixValue y, MatrixValue x)
        {
            var integral = new SimpsonIntegrator(y);
            return integral.Integrate(x);
        }
    }
}
