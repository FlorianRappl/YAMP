using System;
using YAMP.Numerics;

namespace YAMP
{
    class CGFunction : ArgumentFunction
    {
        public Value Function(MatrixValue A, MatrixValue b)
        {
            var cg = new CGSolver(A);
            return cg.Solve(b);
        }

        public Value Function(MatrixValue A, MatrixValue x, MatrixValue b)
        {
            var cg = new CGSolver(A);
            cg.X0 = x;
            return cg.Solve(b);
        }
    }
}
