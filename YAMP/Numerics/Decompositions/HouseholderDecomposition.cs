using System;

namespace YAMP.Numerics
{
    /// <summary>
    /// The Householder reflection is an implementation of a QR decomposition.
    /// This decomposition does not work for complex numbers.
    /// </summary>
    public class HouseholderDecomposition : QRDecomposition
    {
        #region Members

        /// <summary>
        /// Array for internal storage of diagonal of R.
        /// </summary>
        //double[] Rdiag;
        protected ScalarValue[] Rdiag;

        /// <summary>
        /// Array for internal storage of decomposition.
        /// </summary>
        protected ScalarValue[][] QR;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new householder decomposition.
        /// </summary>
        /// <param name="A">The matrix to decompose.</param>
        public HouseholderDecomposition(MatrixValue A)
            : base(A)
        {
            QR = A.GetComplexMatrix();
            Rdiag = new ScalarValue[n];

            // Main loop.
            for (int k = 0; k < n; k++)
            {
                var nrm = 0.0;

                for (int i = k; i < m; i++)
                    nrm = Helpers.Hypot(nrm, QR[i][k].Re);

                if (nrm != 0.0)
                {
                    // Form k-th Householder vector.

                    if (QR[k][k].Re < 0)
                        nrm = -nrm;

                    for (int i = k; i < m; i++)
                        QR[i][k] /= nrm;

                    QR[k][k] += ScalarValue.One;

                    // Apply transformation to remaining columns.
                    for (int j = k + 1; j < n; j++)
                    {
                        var s = ScalarValue.Zero;

                        for (int i = k; i < m; i++)
                            s += QR[i][k] * QR[i][j];

                        s = (-s) / QR[k][k];

                        for (int i = k; i < m; i++)
                            QR[i][j] += s * QR[i][k];
                    }
                }
                else
                    FullRank = false;

                Rdiag[k] = new ScalarValue(-nrm);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Return the upper triangular factor
        /// </summary>
        /// <returns>R</returns>
        public override MatrixValue R
        {
            get
            {
                var X = new MatrixValue(n, n);

                for (int i = 1; i <= n; i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        if (i < j)
                            X[i, j] = QR[i - 1][j - 1];
                        else if (i == j)
                            X[i, j] = Rdiag[i - 1];
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Generate and return the (economy-sized) orthogonal factor
        /// </summary>
        /// <returns>Q</returns>
        public override MatrixValue Q
        {
            get
            {
                var X = new MatrixValue(m, n);

                for (int k = n; k > 0; k--)
                {
                    for (int i = 1; i <= m; i++)
                        X[i, k] = ScalarValue.Zero;

                    X[k, k] = ScalarValue.One;

                    for (int j = k; j <= n; j++)
                    {
                        var l = k - 1;

                        if (QR[l][l] != 0)
                        {
                            var s = ScalarValue.Zero;

                            for (int i = k; i <= m; i++)
                                s += QR[i - 1][l] * X[i, j].Re;

                            s = (-s) / QR[l][l];

                            for (int i = k; i <= m; i++)
                                X[i, j] = X[i, j] + s * QR[i - 1][l];
                        }
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Return the Householder vectors
        /// </summary>
        /// <returns>Lower trapezoidal matrix whose columns define the reflections.</returns>
        public MatrixValue H
        {
            get
            {
                var X = new MatrixValue(m, n);

                for (int i = 1; i <= m; i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        if (i >= j)
                            X[i, j] = QR[i - 1][j - 1];
                    }
                }

                return X;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Least squares solution of A * X = B
        /// </summary>
        /// <param name="b">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X that minimizes the two norm of Q*R*X-B.</returns>
        /// <exception cref="System.ArgumentException"> Matrix row dimensions must agree.</exception>
        /// <exception cref="System.SystemException"> Matrix is rank deficient.</exception>
        public override MatrixValue Solve(MatrixValue b)
        {
            if (b.DimensionY != m)
                throw new YAMPDifferentDimensionsException(m, 1, b.DimensionY, 1);

            if (!this.FullRank)
                throw new YAMPMatrixFormatException(SpecialMatrixFormat.NonSingular);

            // Copy right hand side
            var nx = b.DimensionX;
            var X = b.GetComplexMatrix();

            // Compute Y = transpose(Q)*B
            for (int k = 0; k < n; k++)
            {
                for (int j = 0; j < nx; j++)
                {
                    var s = ScalarValue.Zero;

                    for (int i = k; i < m; i++)
                        s += QR[i][k] * X[i][j];

                    s = (-s) / QR[k][k];

                    for (int i = k; i < m; i++)
                        X[i][j] += s * QR[i][k];
                }
            }

            // Solve R * X = Y;
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

            return new MatrixValue(X, n, nx).GetSubMatrix(0, n, 0, nx);
        }

        #endregion
    }
}
