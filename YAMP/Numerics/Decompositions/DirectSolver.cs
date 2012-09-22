using System;
using YAMP;

namespace YAMP.Numerics
{
    public abstract class DirectSolver
    {
        public abstract MatrixValue Solve(MatrixValue b);
    }
}
