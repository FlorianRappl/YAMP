using System;

namespace YAMP
{
	class NeqOperator : LogicOperator
	{
		public NeqOperator () : base("~=")
		{
		}
		
		public override Operator Create ()
		{
			return new NeqOperator();
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
			return left != right;
		}
	}
}

