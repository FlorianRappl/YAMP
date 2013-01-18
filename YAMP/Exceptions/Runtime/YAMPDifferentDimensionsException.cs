using System;

namespace YAMP
{
    /// <summary>
    /// Class to use when two matrices (or objects) have different dimensions.
    /// </summary>
    public class YAMPDifferentDimensionsException : YAMPRuntimeException
    {
        /// <summary>
        /// Creates a new different dimensions exception.
        /// </summary>
        /// <param name="rowsA">The rows in A.</param>
        /// <param name="columnsA">The columns in A.</param>
        /// <param name="rowsB">The rows in B.</param>
        /// <param name="columnsB">The columns in B.</param>
        public YAMPDifferentDimensionsException(int rowsA, int columnsA, int rowsB, int columnsB)
            : base("Cannot compute the value for two matrices with different dimensions, {0}x{1} and {2}x{3}.", rowsA, columnsA, rowsB, columnsB)
        {
        }

        /// <summary>
        /// Creates a new different dimensions exception.
        /// </summary>
        /// <param name="A">The matrix A.</param>
        /// <param name="B">The matrix B.</param>
        public YAMPDifferentDimensionsException(MatrixValue A, MatrixValue B)
            : this(A.DimensionY, A.DimensionX, B.DimensionY, B.DimensionX)
        {
        }
    }
}
