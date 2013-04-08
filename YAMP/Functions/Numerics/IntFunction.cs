using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Integrates a given vector numerically by summing up all entries and returns the antiderivative vector.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Numerical_integration")]
    sealed class IntFunction : ArgumentFunction
	{
		[Description("Integrates a given vector numerically by summing up all entries and returns the antiderivative vector.")]
        [Example("int([1,2,3,2;2,1,0,-1])", "Integrates the function values given in the matrix and returns the antiderivative matrix [0,1,3,6,8;0,2,3,3,2].")]
        public MatrixValue Function(MatrixValue func)
        {
            var adm = new MatrixValue(func.DimensionY, func.DimensionX + 1);

            for (int i = 1; i <= func.DimensionY; i++)
            {
                adm[i, 1] = new ScalarValue(0);

                for (int t = 1; t <= func.DimensionX; t++)
                {
                    adm[i, t + 1] = new ScalarValue(adm[i, t].Re + func[i, t].Re);
                }
            }

            return adm;
        }
    }
}
