using System;

namespace YAMP
{
    class YAMPDifferentLengthsException : YAMPRuntimeException
    {
        public YAMPDifferentLengthsException(int lengthA, int lengthB)
            : base("Cannot compute the value for two matrices with different lengths, {0} and {1}.", lengthA, lengthB)
        {
        }

        public YAMPDifferentLengthsException(int lengthA, string lengthB)
            : base("Cannot compute the value for the matrix length {0}. The length must be {1}.", lengthA, lengthB)
        {
        }
    }
}
