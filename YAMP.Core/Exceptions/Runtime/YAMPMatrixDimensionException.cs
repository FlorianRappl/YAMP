using System;

namespace YAMP
{
	class YAMPMatrixDimensionException : YAMPRuntimeException
	{
        public YAMPMatrixDimensionException(int rowsRequired, int columnsRequired, int rows, int columns)
            : base("The dimension of the matrix was gives as {0}x{1}, but the dimensions should have been {2}x{3}.", rows, columns, rowsRequired, columnsRequired)
		{
		}
	}
}

