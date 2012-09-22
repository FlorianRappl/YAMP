using System;
using YAMP.Numerics;

namespace YAMP
{
    class SolveFunction : ArgumentFunction
    {
        public Value Function(Value A, Value b)
        {
            if (A is ScalarValue && b is ScalarValue)
            {
                return A.Divide(b);
            }
            else if (A is MatrixValue && b is MatrixValue)
            {
                var M = A as MatrixValue;
                var phi = b as MatrixValue;

                if (M.IsSymmetric)
                {
                    var cg = new CGSolver(M);
                    return cg.Solve(phi);
                }

                //TODO Add more sophisticated algorithms

                return M.Inverse().Multiply(phi);
            }

            throw new ArgumentTypeNotSupportedException("solve");
        }
    }
}
