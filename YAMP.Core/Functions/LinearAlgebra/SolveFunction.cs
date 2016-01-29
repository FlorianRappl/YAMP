using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Solves a system of linear equations by picking the best algorithm.")]
	[Kind(PopularKinds.Function)]
    sealed class SolveFunction : ArgumentFunction
	{
		[Description("Searches a solution vector x for the matrix M and the source vector phi.")]
		[Example("solve(rand(3), rand(3,1))", "Solves the equation M * x = b for a random 3x3 matrix M and a random vector phi.")]
		public MatrixValue Function(MatrixValue M, MatrixValue phi)
		{
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

			return M.Inverse() * phi;
		}

		[Description("Searches a solution vector x for the scalar a and the source b.")]
		[Example("solve(7, 2)", "Solves the equation a * x = b for a scalar a = 7 and a source b = 2.")]
		public ScalarValue Function(ScalarValue a, ScalarValue b)
		{
			return a / b;
		}
	}
}
