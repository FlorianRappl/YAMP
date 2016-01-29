using System;

namespace YAMP
{
	[Description("This is the more general logarithm, i.e. by default the natural logarith, but with the ability to define the basis.")]
	[Kind(PopularKinds.Function)]
    sealed class LogFunction : ArgumentFunction
	{
        [Description("Evaluates the natural logarithm of the given scalar value.")]
        [Example("log(e)", "The natural logarithm for Euler's constant is 1.")]
        public ScalarValue Function(ScalarValue z)
        {
            return GetValue(z, Math.E);
        }

        [Description("Evaluates the custom logarithm of the given scalar value.")]
        [Example("log(16, 2)", "The binary logarithm for the argument 16 is 4.")]
        public ScalarValue Function(ScalarValue z, ScalarValue b)
        {
            return GetValue(z, b.Re);
        }

        [Description("Evaluates the natural logarithm of the values of the given matrix.")]
        [Example("log(randi(10, 0, 1000))", "Generates a 10x10 matrix with integer random numbers between 0 and 1000. Returns a matrix with their natural logarithms.")]
        public MatrixValue Function(MatrixValue Z)
        {
            return Function(Z, new ScalarValue(Math.E));
        }

        [Description("Evaluates the custom logarithm of the values of the given matrix.")]
        [Example("log(randi(10, 0, 1000), 10)", "Generates a 10x10 matrix with integer random numbers between 0 and 1000. Returns a matrix that contains the magnitudes of their values.")]
        public MatrixValue Function(MatrixValue Z, ScalarValue b)
        {
            var bse = b.Re;
            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var j = 1; j <= Z.DimensionY; j++)
                for (var i = 1; i <= Z.DimensionX; i++)
                    M[j, i] = GetValue(Z[j, i], bse);

            return M;
        }

		ScalarValue GetValue(ScalarValue value, double newBase)
		{
			return value.Log(newBase);
		}
	}
}

