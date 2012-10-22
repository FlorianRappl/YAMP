using System;

namespace YAMP
{
	class LtOperator : LogicOperator
	{
		public LtOperator () : base("<")
		{
		}

		public override ScalarValue Compare(ScalarValue left, ScalarValue right)
		{
			return new ScalarValue(left < right);
		}
	}
}

