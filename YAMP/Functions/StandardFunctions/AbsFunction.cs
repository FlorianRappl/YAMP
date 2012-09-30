using System;

namespace YAMP
{
    [Description("Represents the abs function.")]
	class AbsFunction : StandardFunction
	{		
        [Description("Gives the absolute value of the provided scalar or vector, or the determinant of the given matrix.")]
        [Example("abs(3-4)", "Results in 1.")]
        [Example("abs(3+4i)", "Results in 5.")]
        [Example("abs(7i)", "Results in 7.")]
        [Example("abs(1,2;0,4)", "Results in 4.")]
		public override Value Perform (Value argument)
		{
			if(argument is ScalarValue)
			{
				return (argument as ScalarValue).Abs();
			}
			else if(argument is MatrixValue)
			{
				var m = argument as MatrixValue;
				
				if(m.DimensionX == 1)
					return m.Abs();
				else if(m.DimensionY == 1)
					return m.Abs();
				
				return m.Det();
			}
			
			throw new OperationNotSupportedException("abs", argument);
		}
	}
}

