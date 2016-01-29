using System;

namespace YAMP
{
    class YAMPIndexOutOfBoundException : YAMPRuntimeException
    {
        public YAMPIndexOutOfBoundException(int index, int minIndex, int maxIndex)
            : base(minIndex > maxIndex ? "The given matrix is empty. Therefore there are no entries to get."
                : "The index {0} was out of bounds. Only indices between {1} and {2} are allowed.",
                index, minIndex, maxIndex)
        {
        }

        public YAMPIndexOutOfBoundException(int index, int minIndex)
            : base("The index {0} was out of bounds. Only indices greater than {1} are allowed.", index, minIndex)
        {
        }
    }
}
