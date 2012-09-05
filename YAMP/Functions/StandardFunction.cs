using System;
using System.Collections.Generic;

namespace YAMP
{
    abstract class StandardFunction : IFunction
    {
        protected string name;

        public StandardFunction()
        {
            name = GetType().Name.ToLower().Replace("function", string.Empty);
        }

        public virtual Value Perform(Value argument)
        {
            if (argument is ScalarValue)
            {
                return GetValue(argument as ScalarValue);
            }
            else if (argument is MatrixValue)
            {
                var A = argument as MatrixValue;
				var M = new MatrixValue(A.DimensionY, A.DimensionX);
				
				for(var j = 1; j <= A.DimensionY; j++)
					for(var i = 1; i <= A.DimensionX; i++)
						M[j, i] = GetValue(A[j, i]);

                return M;
            }

            throw new OperationNotSupportedException(name, argument);
        }

        protected virtual ScalarValue GetValue(ScalarValue value)
        {
            return value;
        }
    }
}
