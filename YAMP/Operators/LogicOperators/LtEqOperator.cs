using System;

namespace YAMP
{
    /// <summary>
    /// This is the representation of a lighter or equal to operator.
    /// </summary>
    class LtEqOperator : LogicOperator
    {
		public LtEqOperator () : base("<=")
		{
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
			return new ScalarValue(left <= right);
		}

        public override Operator Create()
        {
            return new LtEqOperator();
        }
    }
}
