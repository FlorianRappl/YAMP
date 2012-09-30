using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Solves a system of linear equations by picking the best algorithm.")]
    class SolveFunction : ArgumentFunction
    {
        [Description("Searches a solution vector x for the matrix A and the source vector b.")]
        [Example("solve(A, b)", "Solves the equation A * x = b for a matrix A and a source vector b.")]
        public Value Function(Value A, Value b)
        {
            if (A is ScalarValue && b is ScalarValue)
            {
                return A.Divide(b);
            }
            else if (A is MatrixValue && b is MatrixValue)
            {
                var M = A as MatrixValue;
                var phi = b as MatrixValue;

                if (M.IsSymmetric)
                {
                    var cg = new CGSolver(M);
                    return cg.Solve(phi);
                }
                else if (M.DimensionX == M.DimensionY && M.DimensionY > 64) // Is there a way to "guess" a good number for this?
                {
                    var gmres = new GMRESkSolver(M);
                    gmres.Restart = 30;
                    return gmres.Solve(phi);
                }

                //TODO Add more sophisticated algorithms

                return M.Inverse().Multiply(phi);
            }

            throw new ArgumentTypeNotSupportedException("solve");
        }
    }
}
