using System;
namespace YAMP
{
	public class OperationNotSupportedException : Exception
	{
		public OperationNotSupportedException () : base("The operation is not supported.")
		{
		}
		
		public OperationNotSupportedException (string op) : base("The operation " + op + " is not supported.")
		{
		}
		
		public OperationNotSupportedException (string op, Value reason) : base("The operation " + op + " is not supported with " + reason.ToString() + ".")
		{
		}
	}
}

