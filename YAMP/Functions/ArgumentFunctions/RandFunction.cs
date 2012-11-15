using System;

namespace YAMP
{
	[Description("Generates a matrix with uniformly distributed random values between 0 and 1.")]
	[Kind(PopularKinds.Function)]
	class RandFunction : ArgumentFunction
	{
		static readonly Random ran = new Random();

		[Description("Generates one uniformly dist. random value between 0 and 1.")]
		public ScalarValue Function()
		{
			return new ScalarValue(ran.NextDouble());
		}

		[Description("Generates a n-by-n matrix with uniformly dist. random values between 0 and 1.")]
		[Example("rand(5)", "Generates a 5x5 matrix with an uni. dist. rand. value in each cell.")]
		public MatrixValue Function(ScalarValue dim)
		{
			var k = (int)dim.Value;

			if (k < 1)
				k = 1;
			
			var m = new MatrixValue(k, k);
			
			for(var i = 1; i <= k; i++)
				for(var j = 1; j <= k; j++)
					m[j, i] = new ScalarValue(ran.NextDouble());
			
			return m;
		}

		[Description("Generates a n-by-m matrix with uniformly dist. random values between 0 and 1.")]
		[Example("rand(5, 2)", "Generates a 5x2 matrix with an uni. dist. rand. value in each cell.")]
		public MatrixValue Function(ScalarValue rows, ScalarValue cols)
		{
			var k = (int)rows.Value;
			var l = (int)cols.Value;
			var m = new MatrixValue(k, l);
			
			for(var i = 1; i <= l; i++)
				for(var j = 1; j <= k; j++)
					m[j, i] = new ScalarValue(ran.NextDouble());
			
			return m;
		}
	}
}