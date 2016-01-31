namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The operation invalid exception.
    /// </summary>
	public class YAMPOperationInvalidException : YAMPRuntimeException
	{
		public YAMPOperationInvalidException ()
            : base("The operation is not supported.")
		{
		}

		public YAMPOperationInvalidException(String op)
			: base("The operation {0} is not supported.", op)
		{
		}

        public YAMPOperationInvalidException(String op, Value reason)
			: base("The operation {0} is not supported with {1}.", op, reason.Header)
		{
		}

        public YAMPOperationInvalidException(String op, Value left, Value right)
            : base("The operator {0} is not supported with the operands {1}, {2}.", op, left.Header, right.Header)
        {
        }

        public YAMPOperationInvalidException(String op, String reason)
			: base("The operation {0} is not supported due to {1}.", op, reason)
		{
		}
	}
}

