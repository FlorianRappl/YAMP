using System;

namespace YAMP
{
    public abstract class NumericValue : Value
    {
        public abstract ScalarValue Abs();

        public virtual ScalarValue AbsSquare()
        {
            var abs = Abs();
            return new ScalarValue(abs.Value * abs.Value);
        }

        public abstract void Clear();
    }
}
