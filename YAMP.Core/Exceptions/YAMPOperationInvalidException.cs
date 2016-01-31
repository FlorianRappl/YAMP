namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The operation invalid exception.
    /// </summary>
	public class YAMPOperationInvalidException : YAMPRuntimeException
	{
        internal YAMPOperationInvalidException()
            : base("The operation is not supported.")
		{
		}

        internal YAMPOperationInvalidException(String op)
			: base("The operation {0} is not supported.", op)
		{
		}

        internal YAMPOperationInvalidException(String op, Value reason)
			: base("The operation {0} is not supported with {1}.", op, reason.Header)
		{
		}

        internal YAMPOperationInvalidException(String op, Value left, Value right)
            : base("The operator {0} is not supported with the operands {1}, {2}.", op, left.Header, right.Header)
        {
        }

        internal YAMPOperationInvalidException(String op, String reason)
			: base("The operation {0} is not supported due to {1}.", op, reason)
		{
		}
	}
}

