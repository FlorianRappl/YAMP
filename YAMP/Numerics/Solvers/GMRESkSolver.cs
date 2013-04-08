using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Basic class for a GMRES(k) (with restarts) solver.
    /// </summary>
    public class GMRESkSolver : IterativeSolver
    {
        #region Members

        int i = 0;
        MatrixValue H;
        MatrixValue V;
        MatrixValue gamma;
        MatrixValue c;
        MatrixValue s;

        #endregion

        #region ctor

        /// <summary>
        /// Creates the class for a GMRES(k) solver.
        /// </summary>
        /// <param name="A">The matrix A to solve.</param>
        public GMRESkSolver(MatrixValue A) : this(A, false)
        {
        }

        /// <summary>
        /// Creates the class for a GMRES(k) solver.
        /// </summary>
        /// <param name="A">The matrix A to consider as system of linear equations.</param>
        /// <param name="restart">Should restarts be executed?</param>
        public GMRESkSolver(MatrixValue A, bool restart) : base(A)
        {
            MaxIterations = A.Length;

            if(restart)
                Restart = MaxIterations / 10;
            else //No Restart
                Restart = MaxIterations;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets if restarts should be performed.
        /// </summary>
        public int Restart { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Solves the system of linear equations.
        /// </summary>
        /// <param name="b">The vector b in A * x = b.</param>
        /// <returns>The solution vector x.</returns>
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
            c = new MatrixValue(k - 1, 1);
            s = new MatrixValue(k - 1, 1);
            gamma = new MatrixValue(k + 1, 1);
            var converged = false;

            do
            {
                var j = 0;
                x = X0.Clone();
                var r0 = b - A * x;
                var beta = r0.Abs().Re;

                H.Clear();
                V.Clear();
                gamma.Clear();

                gamma[1] = new ScalarValue(beta);
                c.Clear();
                s.Clear();

                V.SetColumnVector(1, r0 / gamma[1]);

                if (beta < Tolerance)
                    break;

                do
                {
                    j++;
                    i++;

                    var Avj = A * V.GetColumnVector(j);
                    var sum = new MatrixValue(Avj.DimensionY, Avj.DimensionX);

                    for (int m = 1; m <= j; m++)
                    {
                        var w = V.GetColumnVector(m);
                        H[m, j] = w.ComplexDot(Avj);
                        sum += H[m, j] * w;
                    }

                    var wj = Avj - sum;
                    H[j + 1, j] = wj.Abs();
                    Rotate(j);

                    if (H[j + 1, j].AbsSquare() == 0.0)
                    {
                        converged = true;
                        break;
                    }

                    V.SetColumnVector(j + 1, wj / H[j + 1, j]);
                    beta = gamma[j + 1].Abs();

                    if (beta < Tolerance)
                    {
                        converged = true;
                        break;
                    }
                }
                while (j < k);

                var y = new MatrixValue(j, 1);

                for (int l = j; l >= 1; l--)
                {
                    var sum = ScalarValue.Zero;

                    for (int m = l + 1; m <= j; m++)
                        sum += H[l, m] * y[m];

                    y[l] = (gamma[l] - sum) / H[l, l];
                }

                for (int l = 1; l <= j; l++)
                    x += y[l] * V.GetColumnVector(l);

                if (converged)
                    break;

                X0 = x;
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

            var beta = Math.Sqrt(H[j, j].AbsSquare() + H[j + 1, j].AbsSquare());

            s[j] = H[j + 1, j] / beta;
            c[j] = H[j, j] / beta;
            H[j, j] = new ScalarValue(beta);

            gamma[j + 1] = -(s[j] * gamma[j]);
            gamma[j] = c[j].Conjugate() * gamma[j];
        }

        #endregion
    }
}
