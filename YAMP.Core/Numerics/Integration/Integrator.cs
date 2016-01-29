using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// The abstract base class for every integrator algorithm.
    /// </summary>
    public abstract class Integrator
    {
        /// <summary>
        /// Creates a new integrator.
        /// </summary>
        /// <param name="y">The (y) vector to integrate.</param>
        public Integrator(MatrixValue y)
        {
            Values = y;
        }

        /// <summary>
        /// Gets or sets the (y) values used by the integrator.
        /// </summary>
        public MatrixValue Values { get; set; }

        /// <summary>
        /// Performs the integration with the values hold in Values and standard x values.
        /// </summary>
        /// <returns>The result of the integration.</returns>
        public virtual ScalarValue Integrate()
        {
            var x = new RangeValue(1, Values.Length, 1);
            return Integrate(x);
        }

        /// <summary>
        /// Performs the integration with the values hold in Values and the given x values.
        /// </summary>
        /// <param name="x">The x values.</param>
        /// <returns>The result of the integration.</returns>
        public abstract ScalarValue Integrate(MatrixValue x);
    }
}
