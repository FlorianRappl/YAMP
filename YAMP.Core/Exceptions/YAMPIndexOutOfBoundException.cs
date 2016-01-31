namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The index out of bounds exception.
    /// </summary>
    public class YAMPIndexOutOfBoundException : YAMPRuntimeException
    {
        public YAMPIndexOutOfBoundException(Int32 index, Int32 minIndex, Int32 maxIndex)
            : base(minIndex > maxIndex ? "The given matrix is empty. Therefore there are no entries to get."
                : "The index {0} was out of bounds. Only indices between {1} and {2} are allowed.",
                index, minIndex, maxIndex)
        {
        }

        public YAMPIndexOutOfBoundException(Int32 index, Int32 minIndex)
            : base("The index {0} was out of bounds. Only indices greater than {1} are allowed.", index, minIndex)
        {
        }
    }
}
