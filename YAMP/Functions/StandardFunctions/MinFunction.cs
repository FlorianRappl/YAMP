using System;
using System.Collections.Generic;

namespace YAMP
{
	[Description("Computes the minimum value of a given vector or minimum value of each column vector of a matrix.")]
	[Kind(PopularKinds.Function)]
	class MinFunction : StandardFunction
	{
		[Description("Evaluates the vector(s) and outputs the minimum scalar(s) in the vector(s).")]
		[Example("min([1,2,3,4,5,6,7,-1])", "Finds the minimum in the vector, which is -1 in this case.")]
		[Example("min([1,2;3,4;5,6;7,-1])", "Finds the minimums of the vectors (of the matrix), which are 1 and -1 in this case.")]
		public override Value Perform(Value argument)
		{
			if (argument is ScalarValue)
				return argument;
			else if (argument is MatrixValue)
			{
				var m = argument as MatrixValue;

				if(m.DimensionX == 1)
					return GetVectorMin(m.GetColumnVector(1));
				else if(m.DimensionY == 1)
					return GetVectorMin(m.GetRowVector(1));
				else
				{
					var M = new MatrixValue(1, m.DimensionX);
					
					for(var i = 1; i <= m.DimensionX; i++)
						M[1, i] = GetVectorMin(m.GetColumnVector(i));
					
					return M;
				}
			}

			throw new OperationNotSupportedException("min", argument);
		}
		
		ScalarValue GetVectorMin(MatrixValue vec)
		{
			var buf = new ScalarValue();
			var min = double.PositiveInfinity;
			var temp = 0.0;

			for(var i = 1; i <= vec.Length; i++)
			{
				temp = vec[i].Abs().Value;

				if (temp < min)
				{
					buf = vec[i];
					min = temp;
				}
			}

			return buf;
		}
	}
}
