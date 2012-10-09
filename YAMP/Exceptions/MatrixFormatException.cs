using System;

namespace YAMP
{
    public class MatrixFormatException : YAMPException
    {
        public MatrixFormatException(string expected)
            : base("The matrix has to be {0}.", expected)
        {
        }
    }
}
