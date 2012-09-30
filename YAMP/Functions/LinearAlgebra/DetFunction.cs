using System;

namespace YAMP
{
    [Description("Calculates the determinant of the given matrix.")]
	class DetFunction : StandardFunction
	{
        [Description("Uses the best algorithm to compute the determinant.")]
        [Example("det(1,3;-1,0)", "Computes the determinant of the matrix (1,3;-1,0); returns 3.")]
		public override Value Perform (Value argument)
		{
			if(argument is MatrixValue)
				return (argument as MatrixValue).Det();
			else if(argument is ScalarValue)
				return argument;
			
			
			throw new OperationNotSupportedException("det", argument); 
		}
	}
}

