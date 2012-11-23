using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Generates a matrix with uniformly distributed integer values.")]
	[Kind(PopularKinds.Function)]
	class RandiFunction : ArgumentFunction
	{
		static readonly DiscreteUniformDistribution ran = new DiscreteUniformDistribution();

		[Description("Generates one uniformly dist. integer value between min and max.")]
		[Example("randi(0, 10)", "Gets a random integer between 0 and 10 (both inclusive).")]
		public ScalarValue Function (ScalarValue min, ScalarValue max)
		{
			ran.Alpha  = min.IntValue;
			ran.Beta = max.IntValue;
			return new ScalarValue(ran.Next());
		}

		[Description("Generates a n-by-n matrix with uniformly dist. integer values between min and max.")]
		[Example("randi(5, 0, 10)", "Gets a 5x5 matrix with random integers between 0 and 10 (both inclusive).")]
		public MatrixValue Function(ScalarValue dim, ScalarValue min, ScalarValue max)
		{
			ran.Alpha = min.IntValue;
			ran.Beta = max.IntValue;
			var k = (int)dim.Value;

			if (k < 1)
				k = 1;
			
			var m = new MatrixValue(k, k);
			
			for(var i = 1; i <= k; i++)
				for(var j = 1; j <= k; j++)
					m[j, i] = new ScalarValue(ran.Next());
			
			return m;
		}

		[Description("Generates a m-by-n matrix with uniformly dist. integer values between min and max.")]
		[Example("randi(5, 2, 0, 10)", "Gets a 5x2 matrix with random integers between 0 and 10 (both inclusive).")]
		public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue min, ScalarValue max)
		{
			ran.Alpha = min.IntValue;
			ran.Beta = max.IntValue;
			var k = (int)rows.Value;
			var l = (int)cols.Value;
			var m = new MatrixValue(k, l);
			
			for(var i = 1; i <= l; i++)
				for(var j = 1; j <= k; j++)
					m[j, i] = new ScalarValue(ran.Next());
			
			return m;
		}
	}
}

