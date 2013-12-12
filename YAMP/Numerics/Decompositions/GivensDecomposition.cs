using System;

namespace YAMP.Numerics
{
    /// <summary>
    /// The Givens rotation is an implementation of a QR decomposition.
    /// This decomposition also works for complex numbers.
    /// </summary>
    public class GivensDecomposition : QRDecomposition
    {
        #region Members

        MatrixValue q;
        MatrixValue r;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new Givens decomposition.
        /// </summary>
        /// <param name="A">The matrix to decompose.</param>
        public GivensDecomposition(MatrixValue A)
            : base(A)
        {
            var Q = MatrixValue.One(m);
            var R = A.Clone();

            // Main loop.
            for (int j = 1; j < n; j++)
            {
                for (int i = m; i > j; i--)
                {
                    var a = R[i - 1, j];
                    var b = R[i, j];
                    var G = MatrixValue.One(m);

                    var beta = (a * a + b * b).Sqrt();
                    var s = -b / beta;
                    var c = a / beta;

                    G[i - 1, i - 1] = c.Conjugate();
                    G[i - 1, i] = -s.Conjugate();
                    G[i, i - 1] = s;
                    G[i, i] = c;

                    R = G * R;
                    Q = Q * G.Adjungate();
                }
            }

            for (int j = 0; j < n; j++)
            {
                if (R[j + 1, j + 1] == ScalarValue.Zero)
                    FullRank = false;
            }

            r = R;
            q = Q;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Generate and return the (economy-sized) orthogonal factor
        /// </summary>
        /// <returns>Q</returns>
        public override MatrixValue Q
        {
            get { return q; }
        }

        /// <summary>
        /// Return the upper triangular factor
        /// </summary>
        /// <returns>R</returns>
        public override MatrixValue R
        {
            get { return r; }
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

            var cols = b.DimensionX;

            var r = Q.Transpose() * b;
            var x = new MatrixValue(n, cols);

            for (int j = n; j > 0; j--)
            {
                for (int i = 1; i <= cols; i++)
                {
                    var o = ScalarValue.Zero;

                    for (int k = j + 1; k <= n; k++)
                        o += R[j, k] * x[k, i];

                    x[j, i] = (r[j, i] - o) / R[j, j];   
                }
            }

            return x;
        }

        #endregion
    }
}
