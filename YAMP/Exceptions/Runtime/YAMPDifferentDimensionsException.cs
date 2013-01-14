using System;

namespace YAMP
{
    class YAMPDifferentDimensionsException : YAMPRuntimeException
    {
        public YAMPDifferentDimensionsException(int rowsA, int columnsA, int rowsB, int columnsB)
            : base("Cannot compute the value for two matrices with different dimensions, {0}x{1} and {2}x{3}.", rowsA, columnsA, rowsB, columnsB)
        {
        }

        public YAMPDifferentDimensionsException(MatrixValue A, MatrixValue B)
            : base("Cannot compute the value for two matrices with different dimensions, {0}x{1} and {2}x{3}.", A.DimensionY, A.DimensionX, B.DimensionY, B.DimensionX)
        {
        }
    }
}
