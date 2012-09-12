using System;

namespace YAMP
{
    public class MatrixFormatException : Exception
    {
        public MatrixFormatException(string expected) : base("The matrix has to be " + expected + ".")
        {
        }
    }
}
