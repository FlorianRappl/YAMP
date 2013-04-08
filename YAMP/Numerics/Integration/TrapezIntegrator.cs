using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Represents the Trapez integration algorithm - a very simple rule for numerical integration.
    /// </summary>
    public class TrapezIntegrator : Integrator
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="y">The values to integrate.</param>
        public TrapezIntegrator(MatrixValue y) : base(y)
        {
            N = y.Length;
        }

        /// <summary>
        /// Gets the number of values.
        /// </summary>
        public int N
        {
            get;
            private set;
        }

        /// <summary>
        /// Performs the integration.
        /// </summary>
        /// <param name="x">The x values.</param>
        /// <returns>The result of the integration.</returns>
        public override ScalarValue Integrate(MatrixValue x)
        {
            var y = Values;

            if (x.Length != y.Length)
                throw new YAMPDifferentLengthsException(x.Length, y.Length);

            var sum = (x[2].Re - x[1].Re) * y[1] + (x[N].Re - x[N - 1].Re) * y[N];

            for (var i = 2; i < N; i++)
                sum += (x[i + 1].Re - x[i - 1].Re) * y[i].Re;

            return 0.5 * sum;
        }
    }
}
