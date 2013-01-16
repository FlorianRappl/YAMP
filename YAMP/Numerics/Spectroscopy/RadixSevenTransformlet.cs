using System;
using YAMP;

namespace YAMP.Numerics
{
    internal class RadixSevenTransformlet : Transformlet
    {
        public RadixSevenTransformlet(int N, ScalarValue[] u) 
            : base(7, N, u) 
        {
        }

        public override void FftKernel(ScalarValue[] x, ScalarValue[] y, int y0, int dy, int sign)
        {
            // relevent sums and differences
            double a16p = x[1].Re + x[6].Re;
            double a16m = x[1].Re - x[6].Re;
            double a25p = x[2].Re + x[5].Re;
            double a25m = x[2].Re - x[5].Re;
            double a34p = x[3].Re + x[4].Re;
            double a34m = x[3].Re - x[4].Re;
            double b16p = x[1].Im + x[6].Im;
            double b16m = x[1].Im - x[6].Im;
            double b25p = x[2].Im + x[5].Im;
            double b25m = x[2].Im - x[5].Im;
            double b34p = x[3].Im + x[4].Im;
            double b34m = x[3].Im - x[4].Im;

            // combinations used in y[1] and y[6]
            double s16a = x[0].Re + r71.Re * a16p + r72.Re * a25p + r73.Re * a34p;
            double s16b = x[0].Im + r71.Re * b16p + r72.Re * b25p + r73.Re * b34p;
            double t16a = r71.Im * a16m + r72.Im * a25m + r73.Im * a34m;
            double t16b = r71.Im * b16m + r72.Im * b25m + r73.Im * b34m;

            // combinations used in y[2] and y[5]
            double s25a = x[0].Re + r71.Re * a34p + r72.Re * a16p + r73.Re * a25p;
            double s25b = x[0].Im + r71.Re * b34p + r72.Re * b16p + r73.Re * b25p;
            double t25a = r71.Im * a34m - r72.Im * a16m + r73.Im * a25m;
            double t25b = r71.Im * b34m - r72.Im * b16m + r73.Im * b25m;

            // combinations used in y[3] and y[4]
            double s34a = x[0].Re + r71.Re * a25p + r72.Re * a34p + r73.Re * a16p;
            double s34b = x[0].Im + r71.Re * b25p + r72.Re * b34p + r73.Re * b16p;
            double t34a = r71.Im * a25m - r72.Im * a34m - r73.Im * a16m;
            double t34b = r71.Im * b25m - r72.Im * b34m - r73.Im * b16m;

            // if sign is negative, invert t's
            if (sign < 0)
            {
                t16a = -t16a; t16b = -t16b;
                t25a = -t25a; t25b = -t25b;
                t34a = -t34a; t34b = -t34b;
            }

            // combine to get results
            y[y0] = new ScalarValue(x[0].Re + a16p + a25p + a34p, x[0].Im + b16p + b25p + b34p);
            y[y0 + dy] = new ScalarValue(s16a - t16b, s16b + t16a);
            y[y0 + 2 * dy] = new ScalarValue(s25a + t25b, s25b - t25a);
            y[y0 + 3 * dy] = new ScalarValue(s34a + t34b, s34b - t34a);
            y[y0 + 4 * dy] = new ScalarValue(s34a - t34b, s34b + t34a);
            y[y0 + 5 * dy] = new ScalarValue(s25a - t25b, s25b + t25a);
            y[y0 + 6 * dy] = new ScalarValue(s16a + t16b, s16b - t16a);
        }

        static readonly ScalarValue r71 = new ScalarValue(0.62348980185873353053, 0.78183148246802980871);
        static readonly ScalarValue r72 = new ScalarValue(-0.22252093395631440429, 0.97492791218182360702);
        static readonly ScalarValue r73 = new ScalarValue(-0.90096886790241912624, 0.43388373911755812048);

    }
}
