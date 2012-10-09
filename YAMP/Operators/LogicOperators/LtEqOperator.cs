using System;

namespace YAMP
{
    class LtEqOperator : LogicOperator
    {
		public LtEqOperator () : base("<=")
		{
		}
		
		public override Operator Create ()
		{
            return new LtEqOperator();
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
			return new ScalarValue(left <= right);
		}
    }
}
