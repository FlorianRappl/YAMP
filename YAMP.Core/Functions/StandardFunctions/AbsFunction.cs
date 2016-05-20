namespace YAMP
{
    using YAMP.Exceptions;

	[Description("AbsFunctionDescription")]
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
            {
                return new ScalarValue(((ScalarValue)argument).Abs());
            }
            else if (argument is MatrixValue)
            {
                var m = argument as MatrixValue;

                if (m.IsVector)
                {
                    return m.Abs();
                }

                return m.Det();
            }

            throw new YAMPOperationInvalidException("abs", argument);
        }

        [Description("AbsFunctionDescriptionForScalar")]
        [Example("abs(3-4)", "AbsFunctionExampleForScalar1")]
        [Example("abs(3+4i)", "AbsFunctionExampleForScalar2")]
        [Example("abs(7i)", "AbsFunctionExampleForScalar3")]
        public override ScalarValue Function(ScalarValue x)
        {
            return base.Function(x);
        }

        [Description("AbsFunctionDescriptionForMatrix")]
        [Example("abs([1,2;0,4])", "AbsFunctionExampleForMatrix1")]
        [Example("abs([1,2,3])", "AbsFunctionExampleForMatrix2")]
        public override MatrixValue Function(MatrixValue x)
        {
            return base.Function(x);
        }
	}
}

