namespace YAMP
{
    using System;
    using YAMP.Exceptions;

	[Description("MinFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class MinFunction : StandardFunction
	{
		public override Value Perform(Value argument)
		{
            if (argument is ScalarValue)
            {
                return argument;
            }

			if (argument is MatrixValue)
			{
				var m = argument as MatrixValue;

                if (m.DimensionX == 1)
                {
                    return GetVectorMin(m.GetColumnVector(1));
                }
                else if (m.DimensionY == 1)
                {
                    return GetVectorMin(m.GetRowVector(1));
                }
                else
                {
                    var M = new MatrixValue(1, m.DimensionX);

                    for (var i = 1; i <= m.DimensionX; i++)
                    {
                        M[1, i] = GetVectorMin(m.GetColumnVector(i));
                    }

                    return M;
                }
			}

			throw new YAMPOperationInvalidException("min", argument);
		}

        [Description("MinFunctionDescriptionForMatrix")]
        [Example("min([1,2,3,4,5,6,7,-1])", "MinFunctionExampleForMatrix1")]
        [Example("min([1,2;3,4;5,6;7,-1])", "MinFunctionExampleForMatrix2")]
        public override MatrixValue Function(MatrixValue x)
        {
            return base.Function(x);
        }
		
		static ScalarValue GetVectorMin(MatrixValue vec)
		{
			var buf = ScalarValue.Zero;
			var min = Double.MaxValue;
			var temp = 0.0;

			for (var i = 1; i <= vec.Length; i++)
			{
				temp = vec.IsComplex ? vec[i].Abs() : vec[i].Re;

				if (temp < min)
				{
					buf = vec[i];
					min = temp;
				}
			}

			return buf;
		}
	}
}
