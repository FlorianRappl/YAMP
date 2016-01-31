namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The types not equal exception.
    /// </summary>
	public class YAMPTypesNotEqualException : YAMPRuntimeException
	{
        public YAMPTypesNotEqualException(String leftType, String rightType)
            : base("The types {0} and {1} must be equal, however, they are not.", leftType, rightType)
		{
		}
	}
}

