using System;
namespace YAMP
{
	public class OperationNotSupportedException : YAMPException
	{
		public OperationNotSupportedException () : base("The operation is not supported.")
		{
		}

        public OperationNotSupportedException(string op)
            : base("The operation {0} is not supported.", op)
		{
            Symbol = op;
		}

        public OperationNotSupportedException(string op, Value reason)
            : base("The operation {0} is not supported with {1}.", op, reason)
		{
            Symbol = op;
		}

        public OperationNotSupportedException(string op, string reason)
            : base("The operation {0} is not supported {1}.", op, reason)
        {
            Symbol = op;
        }
	}
}

