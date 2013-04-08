using System;
using System.Collections.Generic;

namespace YAMP
{
	[Description("Computes the maximum value of a given vector or maximum value of each column vector of a matrix.")]
	[Kind(PopularKinds.Function)]
    sealed class MaxFunction : StandardFunction
	{
		public override Value Perform(Value argument)
		{
			if (argument is ScalarValue)
				return argument;
			else if (argument is MatrixValue)
			{
				var m = argument as MatrixValue;

				if (m.DimensionX == 1)
					return GetVectorMax(m.GetColumnVector(1));
				else if (m.DimensionY == 1)
					return GetVectorMax(m.GetRowVector(1));
				
				return Function(m);
			}

			throw new YAMPOperationInvalidException("max", argument);
		}

		[Description("Evaluates the vector(s) and outputs the maximum scalar(s) in the vector(s).")]
		[Example("max([1,2,3,4,5,6,7,-1])", "Finds the maximum in the vector, which is 7 in this case.")]
		[Example("max([1,2;3,4;5,6;7,-1])", "Finds the maximums of the vectors (of the matrix), which are 7 and 6 in this case.")]
		public override MatrixValue Function(MatrixValue m)
		{
			var M = new MatrixValue(1, m.DimensionX);

			for (var i = 1; i <= m.DimensionX; i++)
				M[1, i] = GetVectorMax(m.GetColumnVector(i));

			return M;
		}
		
		ScalarValue GetVectorMax(MatrixValue vec)
		{
			var buf = ScalarValue.Zero;
			var max = double.MinValue;
			var temp = 0.0;

			for(var i = 1; i <= vec.Length; i++)
			{
				temp = vec.IsComplex ? vec[i].Abs() : vec[i].Re;

				if (temp > max)
				{
					buf = vec[i];
					max = temp;
				}
			}

			return buf;
		}
	}
}
