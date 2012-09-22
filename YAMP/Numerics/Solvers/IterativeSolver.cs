using System;
using YAMP;

namespace YAMP.Numerics
{
    public abstract class IterativeSolver : DirectSolver
    {
        public IterativeSolver(MatrixValue A)
        {
            this.A = A;
            MaxIterations = 5 * A.DimensionX * A.DimensionY;
            Tolerance = 1e-10;
        }
        
        public MatrixValue A { get; private set; }

        public int MaxIterations { get; set; }

        public MatrixValue X0 { get; set; }

        public double Tolerance { get; set; }
    }
}
