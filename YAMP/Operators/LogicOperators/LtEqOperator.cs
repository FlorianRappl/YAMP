using System;

namespace YAMP
{
    class LtEqOperator : LogicOperator
    {
		public LtEqOperator () : base("<=")
		{
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
			return new ScalarValue(left <= right);
		}
    }
}
