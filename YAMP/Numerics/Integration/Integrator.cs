using System;
using YAMP;

namespace YAMP.Numerics
{
    public abstract class Integrator
    {
        public Integrator(MatrixValue y)
        {
            Values = y;
        }

        public MatrixValue Values { get; set; }

        public virtual ScalarValue Integrate()
        {
            var x = new RangeValue(1, Values.Length, 1);
            return Integrate(x);
        }

        public abstract ScalarValue Integrate(MatrixValue x);
    }
}
