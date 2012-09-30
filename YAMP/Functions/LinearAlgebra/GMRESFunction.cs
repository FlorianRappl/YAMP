using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Uses the GMRES / GMRES(k) algorithm to solve a linear system of equations.")]
    class GMRESFunction : ArgumentFunction
    {
        [Description("Computes the solution vector x for a given matrix A and a source vector b.")]
        [Example("gmres(A, b)", "Here A is an arbitrary matrix with n columns and b is an arbitrary source vector with n rows.")]
        public MatrixValue Function(MatrixValue A, MatrixValue b)
        {
            var gmres = new GMRESkSolver(A);
            return gmres.Solve(b);
        }

        [Description("Computes the solution vector x for a given matrix A, a specified starting solution x0 (guess) and a source vector b.")]
        [Example("gmres(A, x0, b)", "Here A is an arbitrary matrix with n columns, x0 is an arbitrary start vector with n rows and b is an arbitrary source vector with n rows.")]
        public MatrixValue Function(MatrixValue A, MatrixValue x, MatrixValue b)
        {
            var gmres = new GMRESkSolver(A);
            gmres.X0 = x;
            return gmres.Solve(b);
        }
    }
}
