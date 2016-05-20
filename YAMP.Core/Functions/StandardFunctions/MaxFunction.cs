namespace YAMP
{
    using System;
    using YAMP.Exceptions;

	[Description("MaxFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class MaxFunction : StandardFunction
	{
		public override Value Perform(Value argument)
		{
            if (argument is ScalarValue)
            {
                return argument;
            }
            else if (argument is MatrixValue)
            {
                var m = argument as MatrixValue;

                if (m.DimensionX == 1)
                {
                    return GetVectorMax(m.GetColumnVector(1));
                }
                else if (m.DimensionY == 1)
                {
                    return GetVectorMax(m.GetRowVector(1));
                }

                return Function(m);
            }

			throw new YAMPOperationInvalidException("max", argument);
		}

		[Description("MaxFunctionDescriptionForMatrix")]
		[Example("max([1,2,3,4,5,6,7,-1])", "MaxFunctionExampleForMatrix1")]
		[Example("max([1,2;3,4;5,6;7,-1])", "MaxFunctionExampleForMatrix2")]
		public override MatrixValue Function(MatrixValue m)
		{
			var M = new MatrixValue(1, m.DimensionX);

            for (var i = 1; i <= m.DimensionX; i++)
            {
                M[1, i] = GetVectorMax(m.GetColumnVector(i));
            }

			return M;
		}
		
		static ScalarValue GetVectorMax(MatrixValue vec)
		{
			var buf = ScalarValue.Zero;
			var max = Double.MinValue;
			var temp = 0.0;

			for (var i = 1; i <= vec.Length; i++)
			{
				temp = vec.IsComplex ? vec[i].Abs() : vec[i].Re;

				if (temp > max)
				{
					buf = vec[i];
					max = temp;
				}
			}

			return buf;
		}
	}
}
