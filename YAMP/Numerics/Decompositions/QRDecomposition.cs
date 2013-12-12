using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// QR Decomposition.
    /// For an m-by-n matrix A with m >= n, the QR decomposition is an m-by-n
    /// orthogonal matrix Q and an n-by-n upper triangular matrix R so that
    /// A = Q * R.
    /// The QR decompostion always exists, even if the matrix does not have
    /// full rank, so the constructor will never fail.  The primary use of the
    /// QR decomposition is in the least squares solution of nonsquare systems
    /// of simultaneous linear equations.  This will fail if IsFullRank()
    /// returns false.
    /// </summary>
    public abstract class QRDecomposition : DirectSolver
    {
        #region Members

        /// <summary>
        /// Row and column dimensions.
        /// </summary>
        protected int m, n;

        #endregion //  Class variables

        #region Constructor

        /// <summary>
        /// QR Decomposition, computed by Householder reflections.
        /// </summary>
        /// <param name="A">Rectangular matrix</param>
        /// <returns>Structure to access R and the Householder vectors and compute Q.</returns>
        protected QRDecomposition(MatrixValue A)
        {
            // Initialize.
            FullRank = true;
            m = A.DimensionY;
            n = A.DimensionX;
        }

        /// <summary>
        /// Creates the right QR decomposition (Givens or Householder) depending on the given matrix.
        /// </summary>
        /// <param name="A">The matrix to decompose.</param>
        /// <returns>The right QR decomposition implementation.</returns>
        public static QRDecomposition Create(MatrixValue A)
        {
            if (A.IsComplex)
                return new GivensDecomposition(A);

            return new HouseholderDecomposition(A);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Is the matrix full rank?
        /// </summary>
        /// <returns>True if R, and hence A, has full rank.</returns>
        public bool FullRank
        {
            get;
            protected set;
        }

        /// <summary>
        /// Return the upper triangular factor
        /// </summary>
        /// <returns>R</returns>
        public abstract MatrixValue R
        {
            get;
        }

        /// <summary>
        /// Generate and return the (economy-sized) orthogonal factor
        /// </summary>
        /// <returns>Q</returns>
        public abstract MatrixValue Q
        {
            get;
        }

        #endregion
    }
}