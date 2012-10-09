using System;

namespace YAMP
{
    class GtEqOperator : LogicOperator
    {
		public GtEqOperator () : base(">=")
		{
		}
		
		public override Operator Create ()
		{
            return new GtEqOperator();
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
			return new ScalarValue(left >= right);
		}
    }
}
