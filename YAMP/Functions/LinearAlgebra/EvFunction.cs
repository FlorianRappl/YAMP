using System;
using YAMP.Numerics;

namespace YAMP
{
    class EvFunction : StandardFunction
    {
        public override Value Perform(Value argument)
        {
            if (argument is MatrixValue)
            {
                var ev = new Eigenvalues(argument as MatrixValue);
                return ev.GetV();
            }

            throw new OperationNotSupportedException("ev", argument);
        }
    }
}
