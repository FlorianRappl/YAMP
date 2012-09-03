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
            else if (argument is VectorValue)
                return GetVectorMax(argument as VectorValue);
            else if (argument is MatrixValue)
            {
                var m = argument as MatrixValue;
                var l = new List<VectorValue>();

                foreach (var vec in m.Values)
                    l.Add(new VectorValue(GetVectorMax(vec)));

                return new MatrixValue(l);
            }

            throw new OperationNotSupportedException("max", argument);
        }

        ScalarValue GetVectorMax(VectorValue vec)
        {
            var buf = new ScalarValue();
            var max = double.NegativeInfinity;
            var temp = 0.0;

            foreach (var value in vec.Values)
            {
                temp = value.Abs().Value;

                if (temp > max)
                {
                    buf = value;
                    max = temp;
                }
            }

            return buf;
        }
    }
}
