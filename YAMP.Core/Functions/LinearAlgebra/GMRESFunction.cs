using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Uses the GMRES / GMRES(k) algorithm to solve a linear system of equations.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Generalized_minimal_residual_method")]
    sealed class GMRESFunction : ArgumentFunction
    {
        [Description("Computes the solution vector x for a given matrix A and a source vector b.")]
        [Example("gmres(rand(3), rand(3,1))", "Here A is a random 3x3 matrix and b is a random source vector with 3 rows.")]
        public MatrixValue Function(MatrixValue A, MatrixValue b)
        {
            var gmres = new GMRESkSolver(A);
            return gmres.Solve(b);
        }

        [Description("Computes the solution vector x for a given matrix A, a specified starting solution x0 (guess) and a source vector b.")]
        [Example("gmres(rand(3), [1;0;0], rand(3,1))", "Here A is a random 3x3 matrix, x0 is a start vector with 3 rows (1,0,0) and b is a random source vector with 3 rows.")]
        public MatrixValue Function(MatrixValue A, MatrixValue x, MatrixValue b)
        {
            var gmres = new GMRESkSolver(A);
            gmres.X0 = x;
            return gmres.Solve(b);
        }
    }
}
