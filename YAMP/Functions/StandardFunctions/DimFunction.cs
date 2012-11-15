using System;

namespace YAMP
{
	[Description("Outputs the dimensions of the given object.")]
	[Kind(PopularKinds.Function)]
	class DimFunction : StandardFunction
	{
		[Description("Returns a row vector containing the number of rows (1, 1) and the number of columns (1, 2).")]
		[Example("dim([1,2,3,4,5])", "Results in a vector with the elements 1 and 5, since we have 5 columns and 1 row.")]
		public override Value Perform (Value argument)
		{
			var m = new MatrixValue();
			var dimx = 1;
			var dimy = 1;

			if(argument is MatrixValue)
			{
				var t = argument as MatrixValue;
				dimx = t.DimensionX;
				dimy = t.DimensionY;
			}
			else if(argument is StringValue)
			{
				var t = argument as StringValue;
				dimx = t.Value.Length;
				dimy = 1;
			}
			else if(!(argument is ScalarValue))
				throw new OperationNotSupportedException("length", argument);

			m[1, 1] = new ScalarValue(dimy);
			m[1, 2] = new ScalarValue(dimx);
			return m;
		}
	}
}

