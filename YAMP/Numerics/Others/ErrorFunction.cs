using System;

namespace YAMP.Numerics
{
	public static class ErrorFunction
	{
		const double MAXLOG = -7.09782712893383996732e2;

		public static double Normal(double x)
		{
			double y, z;

			double[] T = {
				9.60497373987051638749E0,
				9.00260197203842689217E1,
				2.23200534594684319226E3,
				7.00332514112805075473E3,
				5.55923013010394962768E4
			};

			double[] U = {
				3.35617141647503099647E1,
				5.21357949780152679795E2,
				4.59432382970980127987E3,
				2.26290000613890934246E4,
				4.92673942608635921086E4
			};

			if (Math.Abs(x) > 1.0)
				return 1.0 - Complementary(x);

			z = x * x;
			y = x * polevl(z, T, 4) / p1evl(z, U, 5);
			return y;
		}

		public static double Complementary(double a)
		{
			double x, y, z, p, q;

			double[] P = {
				2.46196981473530512524E-10,
				5.64189564831068821977E-1,
				7.46321056442269912687E0,
				4.86371970985681366614E1,
				1.96520832956077098242E2,
				5.26445194995477358631E2,
				9.34528527171957607540E2,
				1.02755188689515710272E3,
				5.57535335369399327526E2
			};

			double[] Q = {
				1.32281951154744992508E1,
				8.67072140885989742329E1,
				3.54937778887819891062E2,
				9.75708501743205489753E2,
				1.82390916687909736289E3,
				2.24633760818710981792E3,
				1.65666309194161350182E3,
				5.57535340817727675546E2
			};

			double[] R = {
				5.64189583547755073984E-1,
				1.27536670759978104416E0,
				5.01905042251180477414E0,
				6.16021097993053585195E0,
				7.40974269950448939160E0,
				2.97886665372100240670E0
			};

			double[] S = {
				2.26052863220117276590E0,
				9.39603524938001434673E0,
				1.20489539808096656605E1,
				1.70814450747565897222E1,
				9.60896809063285878198E0,
				3.36907645100081516050E0
			};

			x = Math.Abs(a);

			if (x < 1.0) 
				return 1.0 - Normal(a);

			z = -a * a;

			if (z < MAXLOG)
			{
				if (a < 0)
					return (2.0);
				
				return (0.0);
			}

			z = Math.Exp(z);

			if (x < 8.0)
			{
				p = polevl(x, P, 8);
				q = p1evl(x, Q, 8);
			}
			else
			{
				p = polevl(x, R, 5);
				q = p1evl(x, S, 6);
			}

			y = (z * p) / q;

			if (a < 0) 
				y = 2.0 - y;

			if (y == 0.0)
			{
				if (a < 0) 
					return 2.0;
				
				return (0.0);
			}

			return y;
		}

		static double polevl(double x, double[] coef, int N)
		{
			var ans = coef[0];

			for (int i = 1; i <= N; i++)
				ans = ans * x + coef[i];

			return ans;
		}

		static double p1evl(double x, double[] coef, int N)
		{
			var ans = x + coef[0];

			for (int i = 1; i < N; i++)
				ans = ans * x + coef[i];

			return ans;
		}
	}
}
