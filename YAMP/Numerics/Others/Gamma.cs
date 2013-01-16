using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// This class contains the linear gamma function as well as complex ones
    /// and logarithmic ones.
    /// </summary>
	public static class Gamma
	{
		public static double LinearGamma(double x)
		{
			if (x <= 0.0)
			{
				if (x == Math.Ceiling(x))
					return Double.PositiveInfinity;

				return Math.PI / LinearGamma(-x) / (-x) / Math.Sin(x * Math.PI);
			}

			return Math.Exp(LogGamma(x));
		}

		public static ScalarValue LinearGamma(ScalarValue z)
		{
			if (z.Re < 0.5)
				return Math.PI / LinearGamma(1.0 - z) / (Math.PI * z).Sin();

			return LogGamma(z).Exp();
		}

		public static double LogGamma(double x)
		{
            if (x <= 0.0)
                return double.PositiveInfinity;
            else if (x > 16.0)
                return LogGamma_Stirling(x);

			return LanczosLogGamma(x);
		}

		public static ScalarValue LogGamma(ScalarValue z)
		{
            if (z.Re < 0.0)
                return new ScalarValue(double.PositiveInfinity);

			if (z.Abs() > 15.0)
				return LogGamma_Stirling(z);

			return LanczosLogGamma(z);
		}

		static double LogGamma_Stirling(double x)
		{
			var f = (x - 0.5) * Math.Log(x) - x + Math.Log(2.0 * Math.PI) / 2.0;
			var xsqu = x * x;
			var xp = x;

			for (int i = 1; i < 10; i++)
			{
				var f_old = f;
                f += Helpers.bernoulli_numbers[i] / (2 * i) / (2 * i - 1) / xp;

				if (f == f_old)
					return (f);

				xp *= xsqu;
			}

            throw new YAMPNotConvergedException("gamma");
		}

		static ScalarValue LogGamma_Stirling(ScalarValue z)
		{
			if (z.ImaginaryValue < 0.0)
				return LogGamma_Stirling(z.Conjugate()).Conjugate();

			var f = (z - 0.5) * z.Log() - z + Math.Log(2.0 * Math.PI) / 2.0;
			var reduce = f.ImaginaryValue / (2.0 * Math.PI);
			reduce = f.ImaginaryValue - (int)(reduce) * 2.0 * Math.PI;
			f = new ScalarValue(f.Value, reduce);

			var zsqu = z * z;
			var zp = z.Clone();

            for (int i = 1; i < 10; i++)
			{
				var f_old = f.Clone();
                f += Helpers.bernoulli_numbers[i] / (2 * i) / (2 * i - 1) / zp;

				if (f == f_old)
					return (f);

				zp = zp * zsqu;
			}

            throw new YAMPNotConvergedException("gamma");
		}

		static double LanczosLogGamma(double x)
		{
            var sum = Helpers.lanczosd[0];

            for (int i = 1; i < Helpers.lanczosd.Length; i++)
                sum += Helpers.lanczosd[i] / (x + i);

			sum = 2.0 / Math.Sqrt(Math.PI) * sum / x;
			var xshift = x + 0.5;
            var t = xshift * Math.Log(xshift + Helpers.lanczosr) - x;

			return t + Math.Log(sum);
		}

		static ScalarValue LanczosLogGamma(ScalarValue z)
		{
            var sum = new ScalarValue(Helpers.lanczosd[0], 0.0);

            for (int i = 1; i < Helpers.lanczosd.Length; i++)
                sum += Helpers.lanczosd[i] / (z + i);

			sum = (2.0 / Math.Sqrt(Math.PI)) * (sum / z);
			var zshift = z + 0.5;
            var t = zshift * (zshift + Helpers.lanczosr).Ln() - z;

			return t + sum.Ln();
		}

		public static double Psi(double x)
		{
			if (x <= 0.0)
			{
				if (x == Math.Ceiling(x))
					return Double.NaN;

				return Psi(1.0 - x) - Math.PI / Math.Tan(Math.PI * x);
			}
			else if (x > 16.0)
				return Psi_Stirling(x);

			return LanczosPsi(x);
		}

		static double LanczosPsi(double x)
		{
            var s0 = Helpers.lanczosd[0];
			var s1 = 0.0;

            for (int i = 1; i < Helpers.lanczosd.Length; i++)
			{
				var xi = x + i;
                var st = Helpers.lanczosd[i] / xi;
				s0 += st;
				s1 += st / xi;
			}

			var xx = x + Helpers.lanczosr + 0.5;
            var t = Math.Log(xx) - Helpers.lanczosr / xx - 1.0 / x;

			return (t - s1 / s0);
		}

		static double Psi_Stirling(double x)
		{
			var f = Math.Log(x) - 1.0 / (2.0 * x);
			var xsqu = x * x;
			var xp = xsqu;

			for (int i = 1; i < 10; i++)
			{
				var f_old = f;
                f -= Helpers.bernoulli_numbers[i] / (2 * i) / xp;

				if (f == f_old)
					return (f);

				xp *= xsqu;
			}

            throw new YAMPNotConvergedException("gamma");
		}

		public static double Beta(double a, double b)
		{
			return Math.Exp(LogGamma(a) + LogGamma(b) - LogGamma(a + b));
		}
	}
}
