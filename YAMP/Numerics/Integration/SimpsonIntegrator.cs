using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Represents a specific algorithm for integration - Simpson's rule.
    /// </summary>
    public class SimpsonIntegrator : Integrator
    {
        /// <summary>
        /// Creates a new Simpson integrator.
        /// </summary>
        /// <param name="y">The values to integrate.</param>
        public SimpsonIntegrator(MatrixValue y) : base(y)
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

            var sum = 0.0;

            for (var i = 1; i < N - 1; i+=2)
                sum += (x[i + 2].Re - x[i].Re) * (y[i].Re + 4.0 * y[i + 1].Re + y[i + 2].Re);

            return new ScalarValue(sum / 6.0);
        }
    }
}
