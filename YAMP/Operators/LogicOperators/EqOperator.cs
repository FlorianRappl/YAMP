using System;

namespace YAMP
{
	class EqOperator : LogicOperator
	{
		public EqOperator () : base("==")
		{
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
			return new ScalarValue(left == right);
		}
	}
}

