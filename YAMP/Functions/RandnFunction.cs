using System;

namespace YAMP
{
	class RandnFunction : ArgumentFunction
	{	
		static readonly Random ran = new Random();
		static bool buffered = false;
		static double buffer;
		
		public Value Function()
		{
			return new ScalarValue(Gaussian());
		}
		
		public Value Function(ScalarValue dim)
		{
			var k = (int)dim.Value;
			
			if(k <= 1)
				return new ScalarValue(Gaussian());
			
			var m = new MatrixValue(k, k);
			
			for(var i = 1; i <= k; i++)
				for(var j = 1; j <= k; j++)
					m[j, i] = new ScalarValue(Gaussian());
			
			return m;
		}
		
		public Value Function(ScalarValue rows, ScalarValue cols)
		{
			var k = (int)rows.Value;
			var l = (int)cols.Value;
			var m = new MatrixValue(k, l);
			
			for(var i = 1; i <= l; i++)
				for(var j = 1; j <= k; j++)
					m[j, i] = new ScalarValue(Gaussian());
			
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

