using System;
using System.Collections.Generic;

namespace YAMP
{
    abstract class StandardFunction : IFunction
    {
        string name;

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
            else if (argument is VectorValue)
            {
                var v = argument as VectorValue;
                var r = new VectorValue(GetValues(v.Values));
                return r;
            }
            else if (argument is MatrixValue)
            {
                var m = argument as MatrixValue;
                var l = new List<VectorValue>();

                foreach (var v in m.Values)
                    l.Add(new VectorValue(GetValues(v.Values)));

                return new MatrixValue(l);
            }

            throw new OperationNotSupportedException(name, argument);
        }

        protected virtual IEnumerable<ScalarValue> GetValues(IEnumerable<ScalarValue> values)
        {
            foreach (var value in values)
                yield return GetValue(value);
        }

        protected virtual ScalarValue GetValue(ScalarValue value)
        {
            return value;
        }
    }
}
