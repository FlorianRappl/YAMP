using System;

namespace YAMP
{
	[Kind(PopularKinds.Function)]
	[Description("Creates a diagonal matrix that has the given numeric values on the diagonal.")]
    sealed class DiagFunction : ArgumentFunction
    {
        [Description("Creates a diagonal matrix with the values from the given matrix.")]
        [Example("diag(rand(5))", "Creates a matrix with dimension 25 x 25, containing random values on the diagonal.")]
        [Example("diag(rand(5,1))", "Creates a matrix with dimension 5 x 5, containing random values on the diagonal.")]
        public MatrixValue Function(MatrixValue M)
        {
            var m = new MatrixValue(M.Length, M.Length);

            for (var i = 1; i <= M.Length; i++)
                m[i, i] = M[i].Clone();

            return m;
        }

        [Description("Creates a diagonal matrix with the given value, i.e. just returns the value (1 x 1 diagonal matrix = scalar).")]
        [Example("diag(3)", "Returns the given value, which is 3.")]
        public ScalarValue Function(ScalarValue x)
        {
            return x;
        }

		[Description("Tries to create a diagonal matrix with the given arguments, which must be of numeric nature, i.e. scalars or matrices.")]
		[Example("diag(1,1,1,1)", "Creates the unit matrix with dimension 4.")]
		[Example("diag(1, 1, [0, 1;1, 0], 1, 1)", "Creates a matrix that is close to the unit matrix, except that one block has been rotated in the middle.")]
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
                    throw new YAMPArgumentInvalidException(Name, values[i].Header, i);
			}

			return m;
		}
	}
}
