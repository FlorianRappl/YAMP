using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Differentiates a given vector numerically by differences and returns the derivative vector.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Derivative")]
    sealed class DiffFunction : ArgumentFunction
	{
		[Description("Differentiates a given vector numerically by differences and returns the derivative vector.")]
        [Example("diff([0,1,3,6,8;0,2,3,3,2])", "Differentiates the function values given in the matrix and returns the antiderivative matrix [1,2,3,2;2,1,0,-1].")]
        public MatrixValue Function(MatrixValue func)
        {
            var adm = new MatrixValue(func.DimensionY, func.DimensionX - 1);

            for (int i = 1; i <= func.DimensionY; i++)
            {
                for (int t = 1; t <= func.DimensionX - 1; t++)
                {
                    adm[i, t] = new ScalarValue(func[i, t + 1].Re - func[i, t].Re);
                }
            }

            return adm;
        }
    }
}
