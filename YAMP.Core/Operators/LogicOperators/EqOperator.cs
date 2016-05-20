namespace YAMP
{
    using System;

    /// <summary>
    /// The basic equals == operator.
    /// </summary>
	class EqOperator : LogicOperator
	{
		public EqOperator ()
            : base("==")
		{
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
            return new ScalarValue(left == right);
		}

        public override Value Perform(Value left, Value right)
        {
            if (left is StringValue || right is StringValue)
            {
                return new ScalarValue(left.ToString(Context) == right.ToString(Context));
            }
            
            return base.Perform(left, right);
        }

        public override Operator Create()
        {
            return new EqOperator();
        }
	}
}

