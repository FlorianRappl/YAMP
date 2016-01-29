using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Interpolates points between given sample values.")]
	[Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Spline_(mathematics)")]
    sealed class SplineFunction : ArgumentFunction
    {
        [Description("Generates the y value for a single point with given sample data.")]
        [Example("spline([-3,9;-2,4;-1,1;0,0;1,1;3,9], 2)", "Interpolates the y value for x = 2 in this quadratic function. The final outcome is slightly greater than 4.")]
        public MatrixValue Function(MatrixValue original, ScalarValue x)
        {
            var spline = new SplineInterpolation(original);
            var M = new MatrixValue(1, 2);
            M[1, 1].Re = x.Re;
            M[1, 2].Re = spline.ComputeValue(x.Re);
            return M;
        }

        [Description("Generates the y values for a vector of points with given sample data.")]
        [Example("spline([-3,9;-2,4;-1,1;0,0;1,1;3,9], [-1.5, -0.5, 0, 0.5, 1.5])", "Interpolates the y values for this x vector in the quadratic function. The final outcome is a small derivation of the real values.")]
        public MatrixValue Function(MatrixValue original, MatrixValue x)
        {
            var spline = new SplineInterpolation(original);
            return spline.ComputeValues(x);
        }
    }
}
