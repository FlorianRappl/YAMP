using System;

namespace YAMP
{
	class DimFunction : StandardFunction
	{
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

