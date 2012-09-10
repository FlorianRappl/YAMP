using System;

namespace YAMP
{
	class SumFunction : StandardFunction
	{
		public override Value Perform(Value argument)
		{
			if (argument is ScalarValue)
				return argument;
			else if (argument is MatrixValue)
			{
				var m = argument as MatrixValue;
				
				if(m.DimensionX == 1)
					return GetVectorSum(m.GetColumnVector(1));
				else if(m.DimensionY == 1)
					return GetVectorSum(m.GetRowVector(1));
				else
				{
					var M = new MatrixValue(1, m.DimensionX);
					
					for(var i = 1; i <= m.DimensionX; i++)
						M[1, i] = GetVectorSum(m.GetColumnVector(i));
					
					return M;
				}
			}
			
			throw new OperationNotSupportedException("sum", argument);
		}
		
		ScalarValue GetVectorSum(MatrixValue vec)
		{
			var sum = new ScalarValue(0.0);
			
			for(var i = 1; i <= vec.Length; i++)
				sum = sum + vec[i];
			
			return sum;
		}
	}
}

