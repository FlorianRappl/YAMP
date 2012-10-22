using System;

namespace YAMP
{
    class GtEqOperator : LogicOperator
    {
		public GtEqOperator () : base(">=")
		{
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
			return new ScalarValue(left >= right);
		}
    }
}
