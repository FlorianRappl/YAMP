namespace YAMP
{
    [Description("Plot3FunctionDescription")]
    [Kind(PopularKinds.Plot)]
    sealed class Plot3Function : VisualizationFunction
    {
        public Plot3Function(ParseContext context)
            : base(context)
        {
        }

        [Description("Plot3FunctionDescriptionForMatrixMatrixMatrix")]
        [Example("t = 0:pi/50:10*pi; plot3(sin(t), cos(t), t)", "Plot3FunctionExampleForMatrixMatrixMatrix1")]
        public Plot3DValue Function(MatrixValue X, MatrixValue Y, MatrixValue Z)
        {
            var p3 = new Plot3DValue();
            p3.AddPoints(X, Y, Z);
            return p3;
        }
    }
}
