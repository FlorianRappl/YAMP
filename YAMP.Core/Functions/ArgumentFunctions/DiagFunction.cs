namespace YAMP
{
    using YAMP.Exceptions;

	[Kind(PopularKinds.Function)]
	[Description("DiagFunctionDescription")]
    sealed class DiagFunction : ArgumentFunction
    {
        [Description("DiagFunctionDescriptionForMatrix")]
        [Example("diag(rand(5))", "DiagFunctionExampleForMatrix1")]
        [Example("diag(rand(5,1))", "DiagFunctionExampleForMatrix2")]
        public MatrixValue Function(MatrixValue M)
        {
            var m = new MatrixValue(M.Length, M.Length);

            for (var i = 1; i <= M.Length; i++)
            {
                m[i, i] = M[i].Clone();
            }

            return m;
        }

        [Description("DiagFunctionDescriptionForScalar")]
        [Example("diag(3)", "DiagFunctionExampleForScalar1")]
        public ScalarValue Function(ScalarValue x)
        {
            return x;
        }

		[Description("DiagFunctionDescriptionForArguments")]
		[Example("diag(1,1,1,1)", "DiagFunctionExampleForArguments1")]
		[Example("diag(1, 1, [0, 1;1, 0], 1, 1)", "DiagFunctionExampleForArguments2")]
		[Arguments(0, 2)]
		public MatrixValue Function(ArgumentsValue values)
		{
			var m = new MatrixValue();

			for (var i = 1; i <= values.Length; i++)
			{
				var sy = m.DimensionY;
				var sx = m.DimensionX;

                if (values[i] is ScalarValue)
                {
                    var s = (ScalarValue)values[i];
                    m[sy + 1, sx + 1] = s.Clone();
                }
                else if (values[i] is MatrixValue)
                {
                    var n = (MatrixValue)values[i];

                    for (var l = 1; l <= n.DimensionX; l++)
                    {
                        for (var k = 1; k <= n.DimensionY; k++)
                        {
                            m[sy + k, sx + l] = n[k, l].Clone();
                        }
                    }
                }
                else
                {
                    throw new YAMPArgumentInvalidException(Name, values[i].Header, i);
                }
			}

			return m;
		}
	}
}
