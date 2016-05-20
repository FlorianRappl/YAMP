namespace YAMP
{
    using YAMP.Numerics;

    [Description("SolveFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class SolveFunction : ArgumentFunction
	{
		[Description("SolveFunctionDescriptionForMatrixMatrix")]
		[Example("solve(rand(3), rand(3,1))", "SolveFunctionExampleForMatrixMatrix1")]
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

		[Description("SolveFunctionDescriptionForScalarScalar")]
		[Example("solve(7, 2)", "SolveFunctionExampleForScalarScalar1")]
		public ScalarValue Function(ScalarValue a, ScalarValue b)
		{
			return a / b;
		}
	}
}
