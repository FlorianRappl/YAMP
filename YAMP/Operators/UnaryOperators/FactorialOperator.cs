using System;

namespace YAMP
{
	class FactorialOperator : UnaryOperator
	{
		static readonly FactorialFunction fac = new FactorialFunction();
		
		public FactorialOperator() : base("!", 1000)
		{
		}
		
		public override Value Perform (Value left)
		{
			return fac.Perform(left);
		}
	}
}

