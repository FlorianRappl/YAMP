using System;

namespace YAMP
{
    [Description("Generates a matrix with normally (gaussian) distributed random values.")]
	class RandnFunction : ArgumentFunction
	{	
		static readonly Random ran = new Random();
		static bool buffered = false;
		static double buffer;

        [Description("Generates one normally (gaussian) distributed random value around 0 with standard deviation 1.")]
		public ScalarValue Function()
		{
			return new ScalarValue(Gaussian());
		}

        [Description("Generates a n-by-n matrix with normally (gaussian) distributed random value around 0 with standard deviation 1.")]
        [Example("randn(3)", "Gives a 3x3 matrix with normally dist. rand. values.")]
        public MatrixValue Function(ScalarValue dim)
		{
            return Function(dim, dim);
		}

        [Description("Generates a m-by-n matrix with normally (gaussian) distributed random value around 0 with standard deviation 1.")]
        [Example("randn(3, 1)", "Gives a 3x1 matrix with normally dist. rand. values.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
		{
            return Function(rows, cols, new ScalarValue(), new ScalarValue(1.0));
		}

        [Description("Generates a m-by-n matrix with normally (gaussian) distributed random value around mu with standard deviation sigma.")]
        [Example("randn(3, 1, 10, 2.5)", "Gives a 3x1 matrix with normally dist. rand. values around 10 with standard deviation sigma.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue mu, ScalarValue sigma)
        {
            var k = (int)rows.Value;
            var l = (int)cols.Value;
            var m = new MatrixValue(k, l);

            for (var i = 1; i <= l; i++)
                for (var j = 1; j <= k; j++)
                    m[j, i] = new ScalarValue(Gaussian(sigma.Value, mu.Value));

            return m;
        }

		double Gaussian()
		{
			return Gaussian(1.0, 0.0);
		}

		double Gaussian(double sigma, double mu)
		{
			if(buffered)
			{
				buffered = false;
				return mu + sigma * buffer;
			}
			
			double s, u, v;
			
			do
			{
				u = ran.NextDouble() * 2.0 - 1.0;
				v = ran.NextDouble() * 2.0 - 1.0;
				s = u * u + v * v;
			} while(s == 0.0 || s >= 1.0);
			
			double a = Math.Sqrt(-2.0 * Math.Log(s) / s);
			double z = u * a;
			buffer = v * a;
			buffered = true;
			return mu + sigma * z;
		}
	}
}

