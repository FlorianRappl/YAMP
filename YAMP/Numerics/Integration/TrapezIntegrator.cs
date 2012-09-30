using System;
using YAMP;

namespace YAMP.Numerics
{
    public class TrapezIntegrator : Integrator
    {
        public TrapezIntegrator(MatrixValue y) : base(y)
        {
            N = y.Length;
        }

        public override ScalarValue Integrate(MatrixValue x)
        {
            var y = Values;

            if (x.Length != y.Length)
                throw new DimensionException(x.Length, y.Length);

            var sum = (x[2].Value - x[1].Value) * y[1] + (x[N].Value - x[N - 1].Value) * y[N];

            for (var i = 2; i < N; i++)
                sum += (x[i + 1].Value - x[i - 1].Value) * y[i].Value;

            return new ScalarValue(0.5 * sum);
        }

        public int N { get; private set; }
    }
}
