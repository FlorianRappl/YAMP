using System;

namespace YAMP
{
	class GtOperator : LogicOperator
	{
		public GtOperator () : base(">")
		{
		}
		
		public override Operator Create ()
		{
			return new GtOperator();
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
			return new ScalarValue(left > right);
		}
	}
}

