using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Uses the conjugate gradient algorithm to solve a linear system of equations.")]
    class CGFunction : ArgumentFunction
    {
        [Description("Computes the solution vector x for a given matrix A and a source vector b.")]
        [Example("cg(A, b)", "Here A is a symmetric positive definite matrix and b is an arbitrary source vector.")]
        public MatrixValue Function(MatrixValue A, MatrixValue b)
        {
            var cg = new CGSolver(A);
            return cg.Solve(b);
        }

        [Description("Computes the solution vector x for a given matrix A, a specified starting solution x0 (guess) and a source vector b.")]
        [Example("cg(A, x0, b)", "Here A is a symmetric positive definite matrix, x0 is an arbitrary start vector and b is an arbitrary source vector.")]
        public MatrixValue Function(MatrixValue A, MatrixValue x, MatrixValue b)
        {
            var cg = new CGSolver(A);
            cg.X0 = x;
            return cg.Solve(b);
        }
    }
}
