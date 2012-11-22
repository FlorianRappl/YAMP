using System;

namespace YAMP
{
	[Kind(PopularKinds.Function)]
	[Description("Creates a diagonal matrix that has the given numeric values on the diagonal.")]
	class DiagFunction : ArgumentFunction
	{
		[Description("Tries to create a diagonal matrix with the given arguments, which must be of numeric nature, i.e. scalars or matrices.")]
		[Example("diag(1,1,1,1)", "Creates the unit matrix with dimension 4.")]
		[Example("diag(1, 1, [0, 1;1, 0], 1, 1)", "Creates a matrix that is close to the unit matrix, except that one block has been rotated in the middle.")]
		[Arguments(0, 1)]
		public MatrixValue Function(ArgumentsValue values)
		{
			var m = new MatrixValue();

			for (var i = 0; i < values.Length; i++)
			{
				var sy = m.DimensionY;
				var sx = m.DimensionX;

				if (values.Values[i] is ScalarValue)
				{
					var s = (ScalarValue)values.Values[i];
					m[sy + 1, sx + 1] = s.Clone();
				}
				else if (values.Values[i] is MatrixValue)
				{
					var n = (MatrixValue)values.Values[i];

					for(var l = 1; l <= n.DimensionX; l++)
					{
						for (var k = 1; k <= n.DimensionY; k++)
						{
							m[sy + k, sx + l] = n[k, l].Clone();
						}
					}
				}
				else
					throw new ArgumentTypeNotSupportedException("diag", i + 1, values.Values[i].GetType());
			}

			return m;
		}
	}
}
