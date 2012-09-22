using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Cholesky Decomposition.
    /// For a symmetric, positive definite matrix A, the Cholesky decomposition
    /// is an lower triangular matrix L so that A = L*L'.
    /// If the matrix is not symmetric or positive definite, the constructor
    /// returns a partial decomposition and sets an internal flag that may
    /// be queried by the isSPD() method.
    /// </summary>
    public class CholeskyDecomposition : DirectSolver
    {
        #region Members

        /// <summary>
        /// Array for internal storage of decomposition.
        /// </summary>
        double[][] L;

        /// <summary>
        /// Row and column dimension (square matrix).
        /// </summary>
        int n;

        /// <summary>
        /// Symmetric and positive definite flag.
        /// </summary>
        bool isspd;

        #endregion //  Class variables

        #region Constructor

        /// <summary>
        /// Cholesky algorithm for symmetric and positive definite matrix.
        /// </summary>
        /// <param name="Arg">Square, symmetric matrix.</param>
        /// <returns>Structure to access L and isspd flag.</returns>
        public CholeskyDecomposition(MatrixValue Arg)
        {
            // Initialize.
            double[][] A = Arg.GetRealArray();
            n = Arg.DimensionY;
            L = new double[n][];

            for (int i = 0; i < n; i++)
                L[i] = new double[n];

            isspd = (Arg.DimensionX == n);

            // Main loop.
            for (int j = 0; j < n; j++)
            {
                double[] Lrowj = L[j];
                double d = 0.0;

                for (int k = 0; k < j; k++)
                {
                    double[] Lrowk = L[k];
                    double s = 0.0;

                    for (int i = 0; i < k; i++)
                        s += Lrowk[i] * Lrowj[i];
                    
                    Lrowj[k] = s = (A[j][k] - s) / L[k][k];
                    d = d + s * s;
                    isspd = isspd & (A[k][j] == A[j][k]);
                }

                d = A[j][j] - d;
                isspd = isspd & (d > 0.0);
                L[j][j] = Math.Sqrt(Math.Max(d, 0.0));

                for (int k = j + 1; k < n; k++)
                    L[j][k] = 0.0;
            }
        }

        #endregion //  Constructor

        #region Properties
        /// <summary>
        /// Is the matrix symmetric and positive definite?
        /// </summary>
        /// <returns>true if A is symmetric and positive definite.</returns>
        public bool SPD
        {
            get
            {
                return isspd;
            }
        }
        #endregion   // Public Properties

        #region Public Methods

        /// <summary>
        /// Return triangular factor.
        /// </summary>
        /// <returns>L</returns>

        public virtual MatrixValue GetL()
        {
            return new MatrixValue(L, n, n);
        }

        /// <summary>Solve A*X = B</summary>
        /// <param name="B">  A Matrix with as many rows as A and any number of columns.
        /// </param>
        /// <returns>     X so that L*L'*X = B
        /// </returns>
        /// <exception cref="System.ArgumentException">  Matrix row dimensions must agree.
        /// </exception>
        /// <exception cref="System.SystemException"> Matrix is not symmetric positive definite.
        /// </exception>

        public override MatrixValue Solve(MatrixValue B)
        {
            if (B.DimensionY != n)
                throw new DimensionException(n, B.DimensionY);

            if (!isspd)
                throw new MatrixFormatException("symmetric positive definite");

            // Copy right hand side.
            double[][] X = B.GetRealArray();
            int nx = B.DimensionX;

            // Solve L*Y = B;
            for (int k = 0; k < n; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = 0; j < nx; j++)
                        X[i][j] -= X[k][j] * L[i][k];
                }

                for (int j = 0; j < nx; j++)
                    X[k][j] /= L[k][k];
            }

            // Solve L'*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < nx; j++)
                    X[k][j] /= L[k][k];

                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < nx; j++)
                        X[i][j] -= X[k][j] * L[k][i];
                }
            }

            return new MatrixValue(X, n, nx);
        }

        #endregion //  Public Methods
    }
}