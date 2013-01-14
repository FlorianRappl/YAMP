using System;

namespace YAMP
{
	[Description("Outputs the length of the given object.")]
	[Kind(PopularKinds.Function)]
	class LengthFunction : StandardFunction
	{
		public override Value Perform (Value argument)
		{
			if(argument is ScalarValue)
				return new ScalarValue(1.0);
			else if(argument is MatrixValue)
				return new ScalarValue((argument as MatrixValue).Length);
			else if(argument is StringValue)
				return new ScalarValue((argument as StringValue).Value.Length);

			throw new YAMPOperationInvalidException("length", argument);
		}

        [Description("Returns a scalar that is basically the number of rows times the number of columns.")]
        [Example("dim([1,2,3,4,5;6,7,8,9,10])", "Results in a scalar value of 10, since we have 5 columns and 2 rows.")]
        public override MatrixValue Function(MatrixValue x)
        {
            return base.Function(x);
        }
	}
}

