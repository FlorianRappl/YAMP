namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The matrix multiply exception.
    /// </summary>
    public class YAMPMatrixMultiplyException : YAMPRuntimeException
    {
        public YAMPMatrixMultiplyException(Int32 dimXA, Int32 dimYB)
            : base("The multiplication of two matrices, where the columns ({0}) of the first matrix are different from the rows ({1}) of the second matrix is not possible.", dimXA, dimYB)
        {
        }
    }
}
