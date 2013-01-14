using System;
using YAMP;

namespace YAMP.Numerics
{
    public class SimpsonIntegrator : Integrator
    {
        public SimpsonIntegrator(MatrixValue y) : base(y)
        {
            N = y.Length;
        }

        public override ScalarValue Integrate(MatrixValue x)
        {
            var y = Values;

            if (x.Length != y.Length)
                throw new YAMPDifferentLengthsException(x.Length, y.Length);

            var sum = 0.0;

            for (var i = 1; i < N - 1; i+=2)
                sum += (x[i + 2].Value - x[i].Value) * (y[i].Value + 4.0 * y[i + 1].Value + y[i + 2].Value);

            return new ScalarValue(sum / 6.0);
        }

        public int N { get; private set; }
    }
}
