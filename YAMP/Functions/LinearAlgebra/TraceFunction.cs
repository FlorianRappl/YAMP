using System;

namespace YAMP
{
    class TraceFunction : StandardFunction
    {
        public override Value Perform(Value argument)
        {
            if (argument is MatrixValue)
                return (argument as MatrixValue).Trace();
            else if (argument is ScalarValue)
                return argument;

            throw new OperationNotSupportedException("trace", argument);
        }
    }
}
