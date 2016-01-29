using System;
using YAMP;

namespace YAMP.Numerics
{
    internal class RadixFiveTransformlet : Transformlet
    {
        public RadixFiveTransformlet(int N, ScalarValue[] u) 
            : base(5, N, u)
        { 
        }

        public override void FftKernel(ScalarValue[] x, ScalarValue[] y, int y0, int dy, int sign)
        {
            // first set of combinations
            double a14p = x[1].Re + x[4].Re;
            double a14m = x[1].Re - x[4].Re;
            double a23p = x[2].Re + x[3].Re;
            double a23m = x[2].Re - x[3].Re;
            double b14p = x[1].Im + x[4].Im;
            double b14m = x[1].Im - x[4].Im;
            double b23p = x[2].Im + x[3].Im;
            double b23m = x[2].Im - x[3].Im;

            // second set of combinations, for v[1] and v[4]
            double s14a = x[0].Re + r51.Re * a14p + r52.Re * a23p;
            double s14b = x[0].Im + r51.Re * b14p + r52.Re * b23p;
            double t14a = r51.Im * a14m + r52.Im * a23m;
            double t14b = r51.Im * b14m + r52.Im * b23m;

            // second set of combinations, for v[2] and v[3]
            double s23a = x[0].Re + r52.Re * a14p + r51.Re * a23p;
            double s23b = x[0].Im + r52.Re * b14p + r51.Re * b23p;
            double t23a = r52.Im * a14m - r51.Im * a23m;
            double t23b = r52.Im * b14m - r51.Im * b23m;

            // take care of sign

            if (sign < 0) 
            { 
                t14a = -t14a;
                t14b = -t14b; 
                t23a = -t23a; 
                t23b = -t23b; 
            }

            // bring together results
            y[y0] = new ScalarValue(x[0].Re + a14p + a23p, x[0].Im + b14p + b23p);
            y[y0 + dy] = new ScalarValue(s14a - t14b, s14b + t14a);
            y[y0 + 2 * dy] = new ScalarValue(s23a - t23b, s23b + t23a);
            y[y0 + 3 * dy] = new ScalarValue(s23a + t23b, s23b - t23a);
            y[y0 + 4 * dy] = new ScalarValue(s14a + t14b, s14b - t14a);
        }

        static readonly double S5 = Math.Sqrt(5.0);
        static readonly ScalarValue r51 = new ScalarValue((S5 - 1.0) / 4.0, Math.Sqrt((5.0 + S5) / 8.0));
        static readonly ScalarValue r52 = new ScalarValue(-(S5 + 1.0) / 4.0, Math.Sqrt((5.0 - S5) / 8.0));
    }
}
