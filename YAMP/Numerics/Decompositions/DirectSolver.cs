using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Abstract base class for any (direct) solver.
    /// </summary>
    public abstract class DirectSolver
    {
        /// <summary>
        /// Solves the given system of linear equations for a source vector b.
        /// </summary>
        /// <param name="b">The vector b in A * x = b.</param>
        /// <returns>The solution vector x.</returns>
        public abstract MatrixValue Solve(MatrixValue b);
    }
}
