using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Generates a matrix with uniformly distributed integer values.")]
    [Kind(PopularKinds.Random)]
    [Link("http://en.wikipedia.org/wiki/Uniform_distribution_(discrete)")]
    sealed class RandiFunction : ArgumentFunction
	{
		static readonly DiscreteUniformDistribution ran = new DiscreteUniformDistribution();

		[Description("Generates one uniformly dist. integer value between min and max.")]
		[Example("randi(0, 10)", "Gets a random integer between 0 and 10 (both inclusive).")]
		public ScalarValue Function (ScalarValue min, ScalarValue max)
        {
            ran.Alpha = min.GetIntegerOrThrowException("min", Name);
			ran.Beta = max.GetIntegerOrThrowException("max", Name);
			return new ScalarValue(ran.Next());
		}

		[Description("Generates a n-by-n matrix with uniformly dist. integer values between min and max.")]
		[Example("randi(5, 0, 10)", "Gets a 5x5 matrix with random integers between 0 and 10 (both inclusive).")]
		public MatrixValue Function(ScalarValue dim, ScalarValue min, ScalarValue max)
        {
            ran.Alpha = min.GetIntegerOrThrowException("min", Name);
            ran.Beta = max.GetIntegerOrThrowException("max", Name);
			var k = dim.GetIntegerOrThrowException("dim", Name);

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
            ran.Alpha = min.GetIntegerOrThrowException("min", Name);
            ran.Beta = max.GetIntegerOrThrowException("max", Name);
            var k = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
			var m = new MatrixValue(k, l);
			
			for(var i = 1; i <= l; i++)
				for(var j = 1; j <= k; j++)
                    m[j, i] = new ScalarValue(ran.Next());
			
			return m;
		}

		public static DiscreteUniformDistribution Generator
		{
			get { return ran; }
		}
	}
}

