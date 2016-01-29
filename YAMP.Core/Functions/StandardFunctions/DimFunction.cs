using System;

namespace YAMP
{
	[Description("Outputs the dimension of the given object.")]
	[Kind(PopularKinds.Function)]
    sealed class DimFunction : ArgumentFunction
	{
		[Description("Returns a scalar containing the dimension of the given matrix.")]
		[Example("dim([1,2,3,4,5])", "Results in the value 5.")]
		public ScalarValue Function(MatrixValue M)
		{
            return new ScalarValue(Math.Max(M.DimensionX, M.DimensionY));
		}

        [Description("Returns a scalar containing the length of the given scalar, which is always 1.")]
        public ScalarValue Function(ScalarValue str)
        {
            return new ScalarValue(1);
        }

        [Description("Returns a scalar containing the length of the given string.")]
        [Example("dim(\"hello\")", "Results in the value 5.")]
        public ScalarValue Function(StringValue str)
        {
            return new ScalarValue(str.Length);
        }
	}
}

