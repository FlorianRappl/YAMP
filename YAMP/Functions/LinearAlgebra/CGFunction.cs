using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Uses the conjugate gradient algorithm to solve a linear system of equations.")]
	[Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Conjugate_gradient_method")]
    sealed class CGFunction : ArgumentFunction
    {
        [Description("Computes the solution vector x for a given matrix A and a source vector b.")]
        [Example("cg([1,2,1;2,3,4;1,4,2], rand(3,1))", "Here A is an arbitrary symmetric positive definite matrix and b is a random source vector.")]
        public MatrixValue Function(MatrixValue A, MatrixValue b)
        {
            var cg = new CGSolver(A);
            return cg.Solve(b);
        }

        [Description("Computes the solution vector x for a given matrix A, a specified starting solution x0 (guess) and a source vector b.")]
        [Example("cg([1,2,1;2,3,4;1,4,2], [1;0;0], rand(3,1))", "Here A is an arbitrary symmetric positive definite matrix, x0 is a start vector (1;0;0) and b is a random source vector.")]
        public MatrixValue Function(MatrixValue A, MatrixValue x, MatrixValue b)
        {
            var cg = new CGSolver(A);
            cg.X0 = x;
            return cg.Solve(b);
        }
    }
}
