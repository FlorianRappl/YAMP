using System;

namespace YAMP
{
	class EqOperator : LogicOperator
	{
		public EqOperator () : base("==")
		{
		}
		
		public override Operator Create ()
		{
			return new EqOperator();
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
			return left == right;
		}
	}
}

