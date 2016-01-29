using System;

namespace YAMP
{
	[Description("Represents the abs function.")]
	[Kind(PopularKinds.Function)]
    sealed class AbsFunction : StandardFunction
	{		
		public override Value Perform (Value argument)
		{
            return Abs(argument);
		}

        public static Value Abs(Value argument)
        {
            if (argument is ScalarValue)
                return new ScalarValue(((ScalarValue)argument).Abs());
            else if (argument is MatrixValue)
            {
                var m = argument as MatrixValue;

                if (m.IsVector)
                    return m.Abs();

                return m.Det();
            }

            throw new YAMPOperationInvalidException("abs", argument);
        }

        [Description("Gives the absolute value of the provided scalar.")]
        [Example("abs(3-4)", "Results in 1.")]
        [Example("abs(3+4i)", "Results in 5.")]
        [Example("abs(7i)", "Results in 7.")]
        public override ScalarValue Function(ScalarValue x)
        {
            return base.Function(x);
        }

        [Description("Gives the absolute value of the provided vector, or the determinant of the given matrix.")]
        [Example("abs([1,2;0,4])", "Results in 4.")]
        [Example("abs([1,2,3])", "Results in the square root of 14.")]
        public override MatrixValue Function(MatrixValue x)
        {
            return base.Function(x);
        }
	}
}

