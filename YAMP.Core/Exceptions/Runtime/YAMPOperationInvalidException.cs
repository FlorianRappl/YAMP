using System;

namespace YAMP
{
	class YAMPOperationInvalidException : YAMPRuntimeException
	{
		public YAMPOperationInvalidException ()
            : base("The operation is not supported.")
		{
		}

		public YAMPOperationInvalidException(string op)
			: base("The operation {0} is not supported.", op)
		{
		}

		public YAMPOperationInvalidException(string op, Value reason)
			: base("The operation {0} is not supported with {1}.", op, reason.Header)
		{
		}

        public YAMPOperationInvalidException(string op, Value left, Value right)
            : base("The operator {0} is not supported with the operands {1}, {2}.", op, left.Header, right.Header)
        {
        }

		public YAMPOperationInvalidException(string op, string reason)
			: base("The operation {0} is not supported due to {1}.", op, reason)
		{
		}
	}
}

