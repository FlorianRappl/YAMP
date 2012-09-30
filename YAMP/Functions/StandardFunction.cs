using System;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// The abstract base class used for all standard functions.
    /// </summary>
    abstract class StandardFunction : IFunction
    {
        protected string name;

        public StandardFunction()
        {
            name = GetType().Name.RemoveFunctionConvention().ToLower();
        }

        public virtual Value Perform(Value argument)
        {
            if (argument is ScalarValue)
                return GetValue(argument as ScalarValue);
            else if (argument is MatrixValue)
            {
                var A = argument as MatrixValue;
				var M = new MatrixValue(A.DimensionY, A.DimensionX);
				
				for(var j = 1; j <= A.DimensionY; j++)
					for(var i = 1; i <= A.DimensionX; i++)
						M[j, i] = GetValue(A[j, i]);

                return M;
            }
			else if(argument is ArgumentsValue)
				throw new ArgumentsException(name, (argument as ArgumentsValue).Length);

            throw new OperationNotSupportedException(name, argument);
        }

        protected virtual ScalarValue GetValue(ScalarValue value)
        {
            return value;
        }
    }
}
