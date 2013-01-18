using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// The abstract base class for any iterative solver.
    /// </summary>
    public abstract class IterativeSolver : DirectSolver
    {
        /// <summary>
        /// The matrix A that contains the description for a system of linear equations.
        /// </summary>
        /// <param name="A">The A in A * x = b.</param>
        public IterativeSolver(MatrixValue A)
        {
            this.A = A;
            MaxIterations = 5 * A.DimensionX * A.DimensionY;
            Tolerance = 1e-10;
        }

        #region Properties

        /// <summary>
        /// Gets the matrix A in A * x = b.
        /// </summary>
        public MatrixValue A { get; private set; }

        /// <summary>
        /// Gets or sets the maximum number of iterations.
        /// </summary>
        public int MaxIterations { get; set; }

        /// <summary>
        /// Gets or sets the starting vector x0.
        /// </summary>
        public MatrixValue X0 { get; set; }

        /// <summary>
        /// Gets or sets the tolerance level (when to stop the iteration?).
        /// </summary>
        public double Tolerance { get; set; }

        #endregion
    }
}
