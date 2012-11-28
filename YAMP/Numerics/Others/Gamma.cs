using System;
using YAMP;

namespace YAMP.Numerics
{
	public static class Gamma
	{
		public static double LinearGamma(double x)
		{
			if (x <= 0.0)
			{
				if (x == Math.Ceiling(x))
					return Double.NaN;

				return Math.PI / LinearGamma(-x) / (-x) / Math.Sin(x * Math.PI);
			}

			return Math.Exp(LogGamma(x));
		}

		public static ScalarValue LinearGamma(ScalarValue z)
		{
			if (z.Value < 0.5)
				return Math.PI / LinearGamma(1.0 - z) / (Math.PI * z).Sin();

			return LogGamma(z).Exp();
		}

		public static double LogGamma(double x)
		{
			if (x <= 0.0)
				throw new ArgumentOutOfRangeException("x");
			else if (x > 16.0)
				return LogGamma_Stirling(x);

			return LanczosLogGamma(x);
		}

		public static ScalarValue LogGamma(ScalarValue z)
		{
			if (z.Value < 0.0)
				throw new ArgumentOutOfRangeException("z");

			if (z.Abs().Value > 15.0)
				return LogGamma_Stirling(z);

			return LanczosLogGamma(z);
		}

		static double LogGamma_Stirling(double x)
		{
			var f = (x - 0.5) * Math.Log(x) - x + Math.Log(2.0 * Math.PI) / 2.0;
			var xsqu = x * x;
			var xp = x;

			for (int i = 1; i < BernoulliConstant.Bernoulli.Length; i++)
			{
				var f_old = f;
				f += BernoulliConstant.Bernoulli[i] / (2 * i) / (2 * i - 1) / xp;

				if (f == f_old)
					return (f);

				xp *= xsqu;
			}

			throw new NonconvergenceException();
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

			for (int i = 1; i < BernoulliConstant.Bernoulli.Length; i++)
			{
				var f_old = f.Clone();
				f += BernoulliConstant.Bernoulli[i] / (2 * i) / (2 * i - 1) / zp;

				if (f == f_old)
					return (f);

				zp = zp * zsqu;
			}

			throw new NonconvergenceException();
		}

		static double LanczosLogGamma(double x)
		{
			var sum = LanczosD[0];

			for (int i = 1; i < LanczosD.Length; i++)
				sum += LanczosD[i] / (x + i);

			sum = 2.0 / Math.Sqrt(Math.PI) * sum / x;
			var xshift = x + 0.5;
			var t = xshift * Math.Log(xshift + LanczosR) - x;

			return t + Math.Log(sum);
		}

		static ScalarValue LanczosLogGamma(ScalarValue z)
		{
			var sum = new ScalarValue(LanczosD[0], 0.0);

			for (int i = 1; i < LanczosD.Length; i++)
				sum += LanczosD[i] / (z + i);

			sum = (2.0 / Math.Sqrt(Math.PI)) * (sum / z);
			var zshift = z + 0.5;
			var t = zshift * (zshift + LanczosR).Ln() - z;

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
			var s0 = LanczosD[0];
			var s1 = 0.0;

			for (int i = 1; i < LanczosD.Length; i++)
			{
				var xi = x + i;
				var st = LanczosD[i] / xi;
				s0 += st;
				s1 += st / xi;
			}

			var xx = x + LanczosR + 0.5;
			var t = Math.Log(xx) - LanczosR / xx - 1.0 / x;

			return (t - s1 / s0);
		}

		static double Psi_Stirling(double x)
		{
			var f = Math.Log(x) - 1.0 / (2.0 * x);
			var xsqu = x * x;
			var xp = xsqu;

			for (int i = 1; i < BernoulliConstant.Bernoulli.Length; i++)
			{
				var f_old = f;
				f -= BernoulliConstant.Bernoulli[i] / (2 * i) / xp;

				if (f == f_old)
					return (f);

				xp *= xsqu;
			}

			throw new NonconvergenceException();
		}

		public static double Beta(double a, double b)
		{
			return Math.Exp(LogGamma(a) + LogGamma(b) - LogGamma(a + b));
		}

		internal static readonly double[] LanczosD = new double[] {
             2.48574089138753565546e-5,
             1.05142378581721974210,
            -3.45687097222016235469,
             4.51227709466894823700,
            -2.98285225323576655721,
             1.05639711577126713077,
            -1.95428773191645869583e-1,
             1.70970543404441224307e-2,
            -5.71926117404305781283e-4,
             4.63399473359905636708e-6,
            -2.71994908488607703910e-9
        };

		internal const double LanczosR = 10.900511;
	}
}
