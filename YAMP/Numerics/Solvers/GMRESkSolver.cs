using System;
using YAMP;

namespace YAMP.Numerics
{
    public class GMRESkSolver : IterativeSolver
    {
        int i = 0;
        MatrixValue H;
        MatrixValue V;
        MatrixValue gamma;
        MatrixValue c;
        MatrixValue s;

        public GMRESkSolver(MatrixValue A) : this(A, false)
        {
        }

        public GMRESkSolver(MatrixValue A, bool restart) : base(A)
        {
            if(restart)
                Restart = MaxIterations / 10;
            else //No Restart
                Restart = MaxIterations;
        }

        public int Restart { get; set; }

        public override MatrixValue Solve(MatrixValue b)
        {
            var k = Restart;
            MatrixValue x;

            if (X0 == null)
                X0 = new MatrixValue(b.DimensionY, b.DimensionX);
            else if (X0.DimensionX != b.DimensionX || X0.DimensionY != b.DimensionY)
                throw new YAMPDifferentDimensionsException(X0, b);

            H = new MatrixValue(k + 1, k);
            V = new MatrixValue(X0.DimensionY, k);
            c = new MatrixValue(k, 1);
            s = new MatrixValue(k, 1);
            gamma = new MatrixValue(k + 1, 1);
            var converged = false;
            var arnoldi = new Arnoldi(A, H, V);

            do
            {
                var j = 0;
                x = X0.Clone();
                var r0 = b - A * x;
                var beta = r0.Abs();

                H.Clear();
                V.Clear();
                gamma.Clear();
                gamma[1] = beta.Clone();
                c.Clear();
                s.Clear();

                V.SetColumnVector(1, r0 / beta);

                if (beta.Value < Tolerance)
                    break;

                do
                {
                    j++;
                    i++;

                    if (arnoldi.Next())
                    {
                        converged = true;
                        break;
                    }

                    Rotate(j);
                    beta = new ScalarValue(gamma[j + 1].Abs());

                    if (beta.Value < Tolerance)
                    {
                        converged = true;
                        break;
                    }
                }
                while (j < k && !converged);

                var y = new MatrixValue(j + 1, 1);

                for (int l = j; l >= 1; l--)
                {
                    var sum = new ScalarValue();

                    for (int m = l + 1; m <= j; m++)
                        sum = sum + (H[l, m] * y[m]);

                    y[l] = (gamma[l] - sum) / H[l, l];
                }

                for (int l = 1; l <= j; l++)
                    x = x + y[l] * V.GetColumnVector(l);

                if (converged)
                    break;
            }
            while (i < MaxIterations);

            return x;
        }

        void Rotate(int j)
        {
            for (int i = 1; i < j; i++)
            {
                var v1 = H[i, j];
                var v2 = H[i + 1, j];
                H[i, j] = c[i].Conjugate() * v1 + s[i].Conjugate() * v2;
                H[i + 1, j] = c[i] * v2 - s[i] * v1;
            }

            var beta = new ScalarValue(Math.Sqrt(H[j, j].AbsSquare() + H[j + 1, j].AbsSquare()));

            s[j] = H[j + 1, j] / beta;
            c[j] = H[j, j] / beta;
            H[j, j] = beta;

            gamma[j + 1] = -(s[j] * gamma[j]);
            gamma[j] = c[j].Conjugate() * gamma[j];
        }
    }
}
