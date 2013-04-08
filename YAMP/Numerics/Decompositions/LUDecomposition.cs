using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// LU Decomposition.
    /// For an m-by-n matrix A with m >= n, the LU decomposition is an m-by-n
    /// unit lower triangular matrix L, an n-by-n upper triangular matrix U,
    /// and a permutation vector piv of length m so that A(piv,:) = L*U.
    /// <code>
    /// If m is smaller than n, then L is m-by-m and U is m-by-n.
    /// </code>
    /// The LU decompostion with pivoting always exists, even if the matrix is
    /// singular, so the constructor will never fail.  The primary use of the
    /// LU decomposition is in the solution of square systems of simultaneous
    /// linear equations. This will fail if IsNonSingular() returns false.
    /// </summary>
    public class LUDecomposition : DirectSolver
    {
        #region Members

        /// <summary>
        /// Array for internal storage of decomposition.
        /// </summary>
        ScalarValue[][] LU;

        /// <summary>
        /// Row and column dimensions, and pivot sign.
        /// </summary>
        int m, n, pivsign;

        /// <summary>
        /// Internal storage of pivot vector.
        /// </summary>
        int[] piv;

        #endregion //  Class variables

        #region Constructor

        /// <summary>
        /// LU Decomposition
        /// </summary>
        /// <param name="A">Rectangular matrix</param>
        /// <returns>Structure to access L, U and piv.</returns>
        public LUDecomposition(MatrixValue A)
        {
            // Use a "left-looking", dot-product, Crout / Doolittle algorithm.
            LU = A.GetComplexMatrix();
            m = A.DimensionY;
            n = A.DimensionX;
            piv = new int[m];

            for (int i = 0; i < m; i++)
                piv[i] = i;
            
            pivsign = 1;
            var LUrowi = new ScalarValue[0];
            var LUcolj = new ScalarValue[m];

            // Outer loop.
            for (int j = 0; j < n; j++)
            {
                // Make a copy of the j-th column to localize references.
                for (int i = 0; i < m; i++)
                    LUcolj[i] = LU[i][j];

                // Apply previous transformations.
                for (int i = 0; i < m; i++)
                {
                    LUrowi = LU[i];

                    // Most of the time is spent in the following dot product.
                    var kmax = Math.Min(i, j);
                    var s = ScalarValue.Zero;

                    for (int k = 0; k < kmax; k++)
                        s += LUrowi[k] * LUcolj[k];

					LUrowi[j] = LUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.
                var p = j;

                for (int i = j + 1; i < m; i++)
                {
                    if (LUcolj[i].Abs() > LUcolj[p].Abs())
                        p = i;
                }

                if (p != j)
                {
                    for (int k = 0; k < n; k++)
                    {
                        var t = LU[p][k]; 
                        LU[p][k] = LU[j][k]; 
                        LU[j][k] = t;
                    }
                    
                    var k2 = piv[p];
                    piv[p] = piv[j];
                    piv[j] = k2;
                    pivsign = -pivsign;
                }

                // Compute multipliers.

                if (j < m & LU[j][j] != 0.0)
                {
                    for (int i = j + 1; i < m; i++)
                        LU[i][j] = LU[i][j] / LU[j][j];
                }
            }
        }

        #endregion //  Constructor

        #region Public Properties

        /// <summary>
        /// Is the matrix nonsingular?
        /// </summary>
        /// <returns>true if U, and hence A, is nonsingular.</returns>
        virtual public bool IsNonSingular
        {
            get
            {
                for (int j = 0; j < n; j++)
                {
                    if (LU[j][j] == ScalarValue.Zero)
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Return lower triangular factor
        /// </summary>
        /// <returns>L</returns>
        virtual public MatrixValue L
        {
            get
            {
                var X = new MatrixValue(m, n);

                for (int i = 1; i <= m; i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        if (i > j)
                            X[i, j] = LU[i - 1][j - 1];
                        else if (i == j)
                            X[i, j] = ScalarValue.One;
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Return upper triangular factor
        /// </summary>
        /// <returns>U</returns>
        virtual public MatrixValue U
        {
            get
            {
                var X = new MatrixValue(n, n);

                for (int i = 1; i <= n; i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        if (i <= j)
                            X[i, j] = LU[i - 1][j - 1];
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Return pivot permutation vector
        /// </summary>
        /// <returns>piv</returns>
        virtual public MatrixValue Pivot
        {
            get
            {
				var P = new MatrixValue(m, m);

                for (var i = 1; i <= m; i++)
					P[i, piv[i - 1] + 1] = ScalarValue.One;
                
                return P;
            }
        }

        #endregion //  Public Properties

        #region Public Methods

        /// <summary>
        /// Determinant
        /// </summary>
        /// <returns>det(A)</returns>
        public virtual ScalarValue Determinant()
        {
            if (m != n)
                throw new YAMPMatrixFormatException(SpecialMatrixFormat.Square);
            
            ScalarValue d = new ScalarValue(pivsign);

            for (int j = 0; j < n; j++)
                d = d * LU[j][j];

            return d;
        }

        /// <summary>
        /// Solve A*X = B
        /// </summary>
        /// <param name="B">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X so that L*U*X = B(piv,:)</returns>
        public override MatrixValue Solve(MatrixValue B)
        {
            if (B.DimensionY != m)
                throw new YAMPDifferentDimensionsException(B.DimensionY, 1, m, 1);

            if (!this.IsNonSingular)
                throw new YAMPMatrixFormatException(SpecialMatrixFormat.NonSingular);

            // Copy right hand side with pivoting
            var nx = B.DimensionX;
            var X = B.GetSubMatrix(piv, 0, nx).GetComplexMatrix();

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < n; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = 0; j < nx; j++)
                        X[i][j] -= X[k][j] * LU[i][k];
                }
            }

            // Solve U*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < nx; j++)
                    X[k][j] = X[k][j] / LU[k][k];

                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < nx; j++)
                        X[i][j] -= X[k][j] * LU[i][k];
                }
            }

			return new MatrixValue(X, piv.Length, nx);
        }

        #endregion //  Public Methods
    }
}