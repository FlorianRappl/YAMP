namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The matrix dimension exception.
    /// </summary>
	public class YAMPMatrixDimensionException : YAMPRuntimeException
	{
        public YAMPMatrixDimensionException(Int32 rowsRequired, Int32 columnsRequired, Int32 rows, Int32 columns)
            : base("The dimension of the matrix was gives as {0}x{1}, but the dimensions should have been {2}x{3}.", rows, columns, rowsRequired, columnsRequired)
		{
		}
	}
}

