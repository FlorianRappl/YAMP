using System;
using YAMP.Numerics;

namespace YAMP
{
    class FFTFunction : StandardFunction
    {
        public override Value Perform(Value argument)
        {
            if (argument is ScalarValue)
                return argument;
            else if (argument is MatrixValue)
            {
                var m = argument as MatrixValue;

                if (m.DimensionX == 1 || m.DimensionY == 1)
                    return FFT.ifft(m);

                return FFT.ifft2d(m);
            }

            throw new OperationNotSupportedException("fft", argument);
        }
    }
}
