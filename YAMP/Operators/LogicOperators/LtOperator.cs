using System;

namespace YAMP
{
	class LtOperator : LogicOperator
	{
		public LtOperator () : base("<")
		{
		}

		public override Operator Create ()
		{
			return new LtOperator();
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
			return new ScalarValue(left < right);
		}
	}
}

