using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Basic class for a Conjugant Gradient solver.
    /// </summary>
    public class CGSolver : IterativeSolver
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="A">The matrix A for which to solve.</param>
        public CGSolver(MatrixValue A) : base(A)
        {
        }

        /// <summary>
        /// Solves a system of linear equation using the given matrix A.
        /// </summary>
        /// <param name="b">The source vector b, i.e. A * x = b.</param>
        /// <returns>The solution vector x.</returns>
        public override MatrixValue Solve(MatrixValue b)
        {
            MatrixValue x = X0;

            if (x == null)
                x = new MatrixValue(b.DimensionY, b.DimensionX);
            else if (x.DimensionX != b.DimensionX || x.DimensionY != b.DimensionY)
                throw new YAMPDifferentDimensionsException(x.DimensionY, x.DimensionX, b.DimensionY, b.DimensionX);

            var r = b - A * x;
            var p = r.Clone();
            var l = Math.Max(A.Length, MaxIterations);
            var rsold = r.ComplexDot(r);

            for(var i = 1; i < l; i++)
            {
                var Ap = A * p;
                var alpha = rsold / p.ComplexDot(Ap);
                x = x + alpha * p;
                r = r - alpha * Ap;
                var rsnew = r.ComplexDot(r);

                if (rsnew.Abs() < Tolerance)
                    break;

                p = r + rsnew / rsold * p;
                rsold = rsnew;
            }

            return x;
        }
    }
}
