using System;

namespace YAMP
{
	[Description("Computes the product of a given vector or the sum for each column vector of a matrix.")]
	[Kind(PopularKinds.Function)]
	class ProductFunction : StandardFunction
	{
		public override Value Perform(Value argument)
		{
			if (argument is ScalarValue)
				return argument;
			else if (argument is MatrixValue)
			{
				var m = argument as MatrixValue;
				
				if(m.DimensionX == 1)
					return GetVectorProduct(m.GetColumnVector(1));
				else if(m.DimensionY == 1)
					return GetVectorProduct(m.GetRowVector(1));
				else
				{
					var M = new MatrixValue(1, m.DimensionX);
					
					for(var i = 1; i <= m.DimensionX; i++)
						M[1, i] = GetVectorProduct(m.GetColumnVector(i));
					
					return M;
				}
			}
			
			throw new YAMPOperationInvalidException("product", argument);
		}

        [Description("Evaluates the vector(s) and outputs the product(s) of the vector(s).")]
        [Example("product([1,2,3,4,5,6,7,-1])", "Computes the product of the vector, which is -5040 in this case.")]
        [Example("product([1,2;3,4;5,6;7,-1])", "Computes the product of the two vectors, which are 105 and -48 in this case.")]
        public override MatrixValue Function(MatrixValue x)
        {
            return base.Function(x);
        }
		
		ScalarValue GetVectorProduct(MatrixValue vec)
		{
			var sum = new ScalarValue(1.0);
			
			for(var i = 1; i <= vec.Length; i++)
				sum = sum * vec[i];
			
			return sum;
		}
	}
}

