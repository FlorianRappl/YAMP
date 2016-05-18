namespace YAMP
{
	[Description("ModFunctionDescription")]
	[Kind(PopularKinds.Function)]
    [Link("ModFunctionLink")]
    sealed class ModFunction : ArgumentFunction
	{
		[Description("ModFunctionDescriptionForMatrixScalar")]
		[Example("mod([1,2;3,4], 2)", "ModFunctionExampleForMatrixScalar1")]
		public MatrixValue Function(MatrixValue numerator, ScalarValue denominator)
		{
            return Mod(numerator, denominator);
		}

        [Description("ModFunctionDescriptionForScalarMatrix")]
        [Example("mod(2, [1,2;3,4])", "ModFunctionExampleForScalarMatrix1")]
        public MatrixValue Function(ScalarValue numerator, MatrixValue denominator)
        {
            return Mod(numerator, denominator);
        }

		[Description("ModFunctionDescriptionForScalarScalar")]
		[Example("mod(17, 3)", "ModFunctionExampleForScalarScalar1")]
		public ScalarValue Function(ScalarValue numerator, ScalarValue denominator)
		{
            return numerator % denominator;
        }

        #region Static Methods

        public static MatrixValue Mod(MatrixValue numerator, ScalarValue denominator)
        {
            var m = new MatrixValue(numerator.DimensionY, numerator.DimensionX);

            for (var i = 1; i <= numerator.DimensionX; i++)
            {
                for (var j = 1; j <= numerator.DimensionY; j++)
                {
                    m[j, i] = numerator[j, i] % denominator;
                }
            }

            return m;
        }

        public static MatrixValue Mod(ScalarValue numerator, MatrixValue denominator)
        {
            var m = new MatrixValue(denominator.DimensionY, denominator.DimensionX);

            for (var i = 1; i <= denominator.DimensionX; i++)
            {
                for (var j = 1; j <= denominator.DimensionY; j++)
                {
                    m[j, i] = numerator % denominator[j, i];
                }
            }

            return m;
        }

        #endregion
    }
}
