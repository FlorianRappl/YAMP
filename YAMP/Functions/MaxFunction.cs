using System;
using System.Collections.Generic;

namespace YAMP
{
    class MaxFunction : StandardFunction
    {
        public override Value Perform(Value argument)
        {
            if (argument is ScalarValue)
                return argument;
            else if (argument is MatrixValue)
            {
                var m = argument as MatrixValue;

				if(m.DimensionX == 1)
					return GetVectorMax(m.GetColumnVector(1));
				else if(m.DimensionY == 1)
					return GetVectorMax(m.GetRowVector(1));
				else
				{
					var M = new MatrixValue(1, m.DimensionX);
					
					for(var i = 1; i <= m.DimensionX; i++)
						M[1, i] = GetVectorMax(m.GetColumnVector(i));
					
					return M;
				}
            }

            throw new OperationNotSupportedException("max", argument);
        }
		
        ScalarValue GetVectorMax(MatrixValue vec)
        {
            var buf = new ScalarValue();
            var max = double.NegativeInfinity;
            var temp = 0.0;

            for(var i = 1; i <= vec.Length; i++)
            {
                temp = vec[i].Abs().Value;

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
