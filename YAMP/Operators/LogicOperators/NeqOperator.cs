using System;

namespace YAMP
{
    /// <summary>
    /// This is the not equal to operator ~= (attention: it is NOT !=).
    /// </summary>
	class NeqOperator : LogicOperator
	{
		public NeqOperator () : base("~=")
		{
		}

		public override ScalarValue Compare(ScalarValue l, ScalarValue r)
		{
            return new ScalarValue(l != r);
		}

        public override Value Perform(Value left, Value right)
        {
            if (left is StringValue || right is StringValue)
            {
                return new ScalarValue(left.ToString(Context) != right.ToString(Context));
            }

            return base.Perform(left, right);
        }

        public override Operator Create()
        {
            return new NeqOperator();
        }
	}
}

