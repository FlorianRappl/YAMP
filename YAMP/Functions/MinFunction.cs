using System;
using System.Collections.Generic;

namespace YAMP
{
    class MinFunction : StandardFunction
    {
        public override Value Perform(Value argument)
        {
            if (argument is ScalarValue)
                return argument;
            else if (argument is VectorValue)
                return GetVectorMin(argument as VectorValue);
            else if (argument is MatrixValue)
            {
                var m = argument as MatrixValue;
                var l = new List<VectorValue>();

                foreach (var vec in m.Values)
                    l.Add(new VectorValue(GetVectorMin(vec)));

                return new MatrixValue(l);
            }

            throw new OperationNotSupportedException("min", argument);
        }

        ScalarValue GetVectorMin(VectorValue vec)
        {
            var buf = new ScalarValue();
            var min = double.PositiveInfinity;
            var temp = 0.0;

            foreach (var value in vec.Values)
            {
                temp = value.Abs().Value;

                if (temp < min)
                {
                    buf = value;
                    min = temp;
                }
            }

            return buf;
        }
    }
}
