using System;

namespace YAMP
{
	class ColonOperator : BinaryOperator
	{
		public ColonOperator () : base(";", 1)
		{
		}

        public override Operator Create()
        {
            return new ColonOperator();
        }
		
		public override Value Perform (Value left, Value right)
		{
			if(left is MatrixValue || left is ScalarValue)
				return MatrixValue.Create(left).AddRow(right);
			
			throw new OperationNotSupportedException(";", left);
		}
	}
}

