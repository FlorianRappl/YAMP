using System;

namespace YAMP
{
    class YAMPMatrixMultiplyException : YAMPRuntimeException
    {
        public YAMPMatrixMultiplyException(int dimXA, int dimYB)
            : base("The multiplication of two matrices, where the columns ({0}) of the first matrix are different from the rows ({1}) of the second matrix is not possible.", dimXA, dimYB)
        {
        }
    }
}
