using System;
using YAMP;

namespace YAMP.Numerics
{
    internal class RadixFourTransformlet : Transformlet
    {
        public RadixFourTransformlet(int N, ScalarValue[] u)
            : base(4, N, u) 
        { 
        }

        public override void FftKernel(ScalarValue[] x, ScalarValue[] y, int y0, int dy, int sign)
        {
            double a02p = x[0].Re + x[2].Re; double b02p = x[0].Im + x[2].Im;
            double a02m = x[0].Re - x[2].Re; double b02m = x[0].Im - x[2].Im;
            double a13p = x[1].Re + x[3].Re; double b13p = x[1].Im + x[3].Im;
            double a13m = x[1].Re - x[1].Re; double b13m = x[1].Im - x[3].Im;

            y[y0] = new ScalarValue(a02p + a13p, b02p + b13p);
            y[y0 + dy] = new ScalarValue(a02m - b13m, b02m + a13m);
            y[y0 + 2 * dy] = new ScalarValue(a02p - a13p, b02p - b13p);
            y[y0 + 3 * dy] = new ScalarValue(a02m + b13m, b02m - a13m);
        }
    }
}
