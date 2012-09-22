using System;
using YAMP.Numerics;

namespace YAMP
{
    class EigFunction : StandardFunction
    {
        public override Value Perform(Value argument)
        {
            if (argument is MatrixValue)
            {
                var ev = new Eigenvalues(argument as MatrixValue);
                var m = new MatrixValue(ev.RealEigenvalues.Length, 1);

                for (var i = 1; i <= ev.RealEigenvalues.Length; i++)
                    m[i, 1] = new ScalarValue(ev.RealEigenvalues[i - 1], ev.ImagEigenvalues[i - 1]);

                return m;
            }

            throw new OperationNotSupportedException("eig", argument);
        }
    }
}
