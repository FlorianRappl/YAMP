using System;

namespace YAMP
{
	[Description("Outputs the length of the given object.")]
	[Kind(PopularKinds.Function)]
    sealed class LengthFunction : ArgumentFunction
	{
        [Description("Returns a scalar that is basically the number of rows times the number of columns.")]
        [Example("length([1,2,3,4,5;6,7,8,9,10])", "Results in a scalar value of 10, since we have 5 columns and 2 rows.")]
        public ScalarValue Function(MatrixValue M)
        {
            return new ScalarValue(M.Length);
        }

        [Description("Returns the length of a scalar, which is just 1.")]
        public ScalarValue Function(ScalarValue x)
        {
            return new ScalarValue(1);
        }

        [Description("Returns the length of the string, which is the number of characters.")]
        [Example("length(\"hello\")", "This is evaluated to be 5, which is the number of characters in the given string.")]
        public ScalarValue Function(StringValue str)
        {
            return new ScalarValue(str.Length);
        }
	}
}

