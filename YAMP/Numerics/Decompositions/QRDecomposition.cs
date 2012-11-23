using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// QR Decomposition.
    /// For an m-by-n matrix A with m >= n, the QR decomposition is an m-by-n
    /// orthogonal matrix Q and an n-by-n upper triangular matrix R so that
    /// A = Q*R.
    /// The QR decompostion always exists, even if the matrix does not have
    /// full rank, so the constructor will never fail.  The primary use of the
    /// QR decomposition is in the least squares solution of nonsquare systems
    /// of simultaneous linear equations.  This will fail if IsFullRank()
    /// returns false.
    /// </summary>
    public class QRDecomposition : DirectSolver
    {
        #region Members

        /// <summary>
        /// Array for internal storage of decomposition.
        /// </summary>
        double[][] QR;

        /// <summary>
        /// Row and column dimensions.
        /// </summary>
        int m, n;

        /// <summary>
        /// Array for internal storage of diagonal of R.
        /// </summary>
        double[] Rdiag;

        #endregion //  Class variables

        #region Constructor

        /// <summary>
        /// QR Decomposition, computed by Householder reflections.
        /// </summary>
        /// <param name="A">Rectangular matrix</param>
        /// <returns>Structure to access R and the Householder vectors and compute Q.</returns>
        public QRDecomposition(MatrixValue A)
        {
            // Initialize.
            QR = A.GetRealArray();
            m = A.DimensionY;
            n = A.DimensionX;
            Rdiag = new double[n];

            // Main loop.
            for (int k = 0; k < n; k++)
            {
                // Compute 2-norm of k-th column without under/overflow.
                var nrm = 0.0;

                for (int i = k; i < m; i++)
                    nrm = NumericHelpers.Hypot(nrm, QR[i][k]);

                if (nrm != 0.0)
                {
                    // Form k-th Householder vector.
                    if (QR[k][k] < 0)
                        nrm = -nrm;
                    
                    for (int i = k; i < m; i++)
                        QR[i][k] /= nrm;
                    
                    QR[k][k] += 1.0;

                    // Apply transformation to remaining columns.
                    for (int j = k + 1; j < n; j++)
                    {
                        var s = 0.0;

                        for (int i = k; i < m; i++)
                            s += QR[i][k] * QR[i][j];
                        
                        s = (-s) / QR[k][k];

                        for (int i = k; i < m; i++)
                            QR[i][j] += s * QR[i][k];
                    }
                }

                Rdiag[k] = -nrm;
            }
        }

        #endregion //  Constructor

        #region Public Properties

        /// <summary>
        /// Is the matrix full rank?
        /// </summary>
        /// <returns>true if R, and hence A, has full rank.</returns>
        virtual public bool FullRank
        {
            get
            {
                for (int j = 0; j < n; j++)
                {
                    if (Rdiag[j] == 0)
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Return the Householder vectors
        /// </summary>
        /// <returns>Lower trapezoidal matrix whose columns define the reflections.</returns>
        virtual public MatrixValue H
        {
            get
            {
                var X = new MatrixValue(m, n);

                for (int i = 1; i <= m; i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        if (i >= j)
                            X[i, j] = new ScalarValue(QR[i - 1][j - 1]);
                    }
                }

                return X;
            }

        }

        /// <summary>
        /// Return the upper triangular factor
        /// </summary>
        /// <returns>R</returns>
        virtual public MatrixValue R
        {
            get
            {
                var X = new MatrixValue(n, n);

                for (int i = 1; i <= n; i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        if (i < j)
                            X[i, j] = new ScalarValue(QR[i - 1][j - 1]);
                        else if (i == j)
                            X[i, j] = new ScalarValue(Rdiag[i - 1]);
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Generate and return the (economy-sized) orthogonal factor
        /// </summary>
        /// <returns>Q</returns>
        virtual public MatrixValue Q
        {
            get
            {
                var X = new MatrixValue(m, n);

                for (int k = n; k > 0; k--)
                {
                    for (int i = 1; i <= m; i++)
                        X[i, k] = new ScalarValue(0.0);

                    X[k, k] = new ScalarValue(1.0);

                    for (int j = k; j <= n; j++)
                    {
                        var l = k - 1;

                        if (QR[l][l] != 0)
                        {
                            var s = 0.0;

                            for (int i = k; i <= m; i++)
                                s += QR[i - 1][l] * X[i, j].Value;

                            s = (-s) / QR[l][l];

                            for (int i = k; i <= m; i++)
                                X[i, j] = X[i, j] + s * QR[i - 1][l];
                        }
                    }
                }

                return X;
            }
        }

        #endregion //  Public Properties

        #region Public Methods

        /// <summary>
        /// Least squares solution of A*X = B
        /// </summary>
        /// <param name="B">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X that minimizes the two norm of Q*R*X-B.</returns>
        /// <exception cref="System.ArgumentException"> Matrix row dimensions must agree.</exception>
        /// <exception cref="System.SystemException"> Matrix is rank deficient.</exception>
        public override MatrixValue Solve(MatrixValue B)
        {
            if (B.DimensionY != m)
                throw new DimensionException(B.DimensionY, m);

            if (!this.FullRank)
                throw new MatrixFormatException("full rank");

            // Copy right hand side
            var nx = B.DimensionX;
            var X = B.GetRealArray();

            // Compute Y = transpose(Q)*B
            for (int k = 0; k < n; k++)
            {
                for (int j = 0; j < nx; j++)
                {
                    var s = 0.0;

                    for (int i = k; i < m; i++)
                        s += QR[i][k] * X[i][j];

                    s = (-s) / QR[k][k];

                    for (int i = k; i < m; i++)
                        X[i][j] += s * QR[i][k];
                }
            }

            // Solve R*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < nx; j++)
                    X[k][j] /= Rdiag[k];

                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < nx; j++)
                        X[i][j] -= X[k][j] * QR[i][k];
                }
            }

            return new MatrixValue(X, n, nx).SubMatrix(0, n, 0, nx);
        }

        #endregion //  Public Methods
    }
}