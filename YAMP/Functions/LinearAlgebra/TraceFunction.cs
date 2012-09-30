using System;

namespace YAMP
{
    [Description("Performs the trace operation on the given matrix.")]
    class TraceFunction : StandardFunction
    {
        [Description("Sums all elements on the diagonal.")]
        [Example("trace(1,2;3,4)", "Results in the value 5.")]
        public override Value Perform(Value argument)
        {
            if (argument is MatrixValue)
                return (argument as MatrixValue).Trace();
            else if (argument is ScalarValue)
                return argument as ScalarValue;

            throw new OperationNotSupportedException("trace", argument);
        }
    }
}
