using System;

namespace YAMP
{
    [Description("Inverts the given matrix.")]
	class InvFunction : StandardFunction
	{
        [Description("Tries to find the inverse of the matrix, i.e. inv(A)=A^-1.")]
        [Example("inv(0,2;1,0)", "Inverts the matrix (0,2;1,0), resulting in (0,1;0.5,0).")]
		public override Value Perform (Value argument)
		{
			if(argument is ScalarValue)
				return 1.0 / (argument as ScalarValue);
			else if (argument is MatrixValue)
			{
				var m = argument as MatrixValue;
				return m.Inverse();
			}

			throw new OperationNotSupportedException("inv", argument);
		}
	}
}

