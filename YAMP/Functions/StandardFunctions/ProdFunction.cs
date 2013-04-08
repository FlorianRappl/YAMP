using System;

namespace YAMP
{
	[Description("Computes the product of a given vector or the sum for each column vector of a matrix.")]
	[Kind(PopularKinds.Function)]
    sealed class ProdFunction : ArgumentFunction
    {
        [Description("Just returns the value, since the product of one scalar is the scalar itself.")]
        public Value Function(ScalarValue x)
		{
            return x;
		}

        [Description("Evaluates the vector(s) and outputs the product(s) of the vector(s).")]
        [Example("prod([1,2,3,4,5,6,7,-1])", "Computes the product of the vector, which is -5040 in this case.")]
        [Example("prod([1,2;3,4;5,6;7,-1])", "Computes the product of the two vectors, which are 105 and -48 in this case.")]
        public Value Function(MatrixValue m)
        {
            if (m.DimensionX == 1)
                return GetVectorProduct(m.GetColumnVector(1));
            else if (m.DimensionY == 1)
                return GetVectorProduct(m.GetRowVector(1));
            else
            {
                var M = new MatrixValue(1, m.DimensionX);

                for (var i = 1; i <= m.DimensionX; i++)
                    M[1, i] = GetVectorProduct(m.GetColumnVector(i));

                return M;
            }
        }
		
		ScalarValue GetVectorProduct(MatrixValue vec)
		{
			var prod = ScalarValue.One;
			
			for(var i = 1; i <= vec.Length; i++)
				prod = prod * vec[i];
			
			return prod;
		}
	}
}

