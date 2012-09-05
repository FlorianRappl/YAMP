using System;

namespace YAMP
{
    class IndexOperator : Operator
    {
        public IndexOperator() : base("_", 0)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			if(left is MatrixValue)
				return (left as MatrixValue).Index(right);
			
			throw new OperationNotSupportedException("_", left);
		}
    }
}
