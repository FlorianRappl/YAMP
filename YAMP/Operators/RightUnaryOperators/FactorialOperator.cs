using System;

namespace YAMP
{
    /// <summary>
    /// This is the factorial operator !.
    /// </summary>
	class FactorialOperator : RightUnaryOperator
	{
		static readonly FactorialFunction fac = new FactorialFunction();
		
		public FactorialOperator() : base("!", 1000)
		{
		}
		
		public override Value Perform (Value left)
		{
			return fac.Perform(left);
		}

        public override Operator Create()
        {
            return new FactorialOperator();
        }
	}
}

