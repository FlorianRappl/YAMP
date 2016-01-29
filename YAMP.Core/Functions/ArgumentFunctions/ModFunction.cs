using System;

namespace YAMP
{
	[Description("Calculates the modulo of the integer real parts of the arguments.")]
	[Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Modular_arithmetic")]
    sealed class ModFunction : ArgumentFunction
	{
		[Description("Performes the modulo operation on each of the entries of the given matrix.")]
		[Example("mod([1,2;3,4], 2)", "Computes the modulo matrix resulting in [1,0;1;0].")]
		public MatrixValue Function(MatrixValue numerator, ScalarValue denominator)
		{
            return Mod(numerator, denominator);
		}

        [Description("Performes the modulo operation on each of the entries of the given matrix.")]
        [Example("mod(2, [1,2;3,4])", "Computes the modulo matrix resulting in [0,0;2;2].")]
        public MatrixValue Function(ScalarValue numerator, MatrixValue denominator)
        {
            return Mod(numerator, denominator);
        }

		[Description("Calculates the modulo of the integer real parts of the numerator with respect to the denominator.")]
		[Example("mod(17, 3)", "Computes the modulo of 17 with respect to 3 resulting in 2.")]
		public ScalarValue Function(ScalarValue numerator, ScalarValue denominator)
		{
            return numerator % denominator;
        }

        #region Static Methods

        public static MatrixValue Mod(MatrixValue numerator, ScalarValue denominator)
        {
            var m = new MatrixValue(numerator.DimensionY, numerator.DimensionX);

            for (var i = 1; i <= numerator.DimensionX; i++)
                for (var j = 1; j <= numerator.DimensionY; j++)
                    m[j, i] = numerator[j, i] % denominator;

            return m;
        }

        public static MatrixValue Mod(ScalarValue numerator, MatrixValue denominator)
        {
            var m = new MatrixValue(denominator.DimensionY, denominator.DimensionX);

            for (var i = 1; i <= denominator.DimensionX; i++)
                for (var j = 1; j <= denominator.DimensionY; j++)
                    m[j, i] = numerator % denominator[j, i];

            return m;
        }

        #endregion
    }
}
