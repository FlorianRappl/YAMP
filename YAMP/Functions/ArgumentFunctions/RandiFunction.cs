using System;

namespace YAMP
{
    [Description("Generates a matrix with uniformly distributed integer values.")]
	class RandiFunction : ArgumentFunction
	{
		static readonly Random ran = new Random();

        [Description("Generates one uniformly dist. integer value between min and max.")]
        [Example("randi(0, 10)", "Gets a random integer between 0 and 10 (both inclusive).")]
		public ScalarValue Function (ScalarValue min, ScalarValue max)
		{
			return new ScalarValue(ran.Next(min.IntValue, max.IntValue + 1));
		}

        [Description("Generates a n-by-n matrix with uniformly dist. integer values between min and max.")]
        [Example("randi(5, 0, 10)", "Gets a 5x5 matrix with random integers between 0 and 10 (both inclusive).")]
        public MatrixValue Function(ScalarValue dim, ScalarValue min, ScalarValue max)
		{
			var k = (int)dim.Value;

            if (k < 1)
                k = 1;
			
			var m = new MatrixValue(k, k);
			
			for(var i = 1; i <= k; i++)
				for(var j = 1; j <= k; j++)
					m[j, i] = new ScalarValue(ran.Next(min.IntValue, max.IntValue + 1));
			
			return m;
		}

        [Description("Generates a m-by-n matrix with uniformly dist. integer values between min and max.")]
        [Example("randi(5, 2, 0, 10)", "Gets a 5x2 matrix with random integers between 0 and 10 (both inclusive).")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue min, ScalarValue max)
		{
			var k = (int)rows.Value;
			var l = (int)cols.Value;
			var m = new MatrixValue(k, l);
			
			for(var i = 1; i <= l; i++)
				for(var j = 1; j <= k; j++)
					m[j, i] = new ScalarValue(ran.Next(min.IntValue, max.IntValue + 1));
			
			return m;
		}
	}
}

