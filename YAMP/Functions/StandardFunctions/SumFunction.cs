using System;

namespace YAMP
{
	[Description("Computes the sum of a given vector or the sum for each column vector of a matrix.")]
	[Kind(PopularKinds.Function)]
    sealed class SumFunction : ArgumentFunction
    {
        [Description("Just returns the value, since the sum of one scalar is the scalar itself.")]
        public ScalarValue Function(ScalarValue x)
		{
            return x;
		}

        [Description("Evaluates the vector(s) and outputs the sum(s) of the vector(s).")]
        [Example("sum([1,2,3,4,5,6,7,-1])", "Computes the sum of the vector, which is 27 in this case.")]
        [Example("sum([1,2;3,4;5,6;7,-1])", "Computes the sums of the two vectors, which are 16 and 11 in this case.")]
        public Value Function(MatrixValue m)
        {
            if (m.DimensionX == 1)
                return GetVectorSum(m.GetColumnVector(1));
            else if (m.DimensionY == 1)
                return GetVectorSum(m.GetRowVector(1));
            else
            {
                var M = new MatrixValue(1, m.DimensionX);

                for (var i = 1; i <= m.DimensionX; i++)
                    M[1, i] = GetVectorSum(m.GetColumnVector(i));

                return M;
            }
        }
		
		ScalarValue GetVectorSum(MatrixValue vec)
		{
			var sum = ScalarValue.Zero;
			
			for(var i = 1; i <= vec.Length; i++)
				sum = sum + vec[i];
			
			return sum;
		}
	}
}

