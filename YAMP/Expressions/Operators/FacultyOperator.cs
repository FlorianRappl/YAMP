using System;

namespace YAMP
{
	class FacultyOperator : UnaryOperator
	{
		static FacultyFunction fac = new FacultyFunction();
		
		public FacultyOperator () : base("!", 1000)
		{
		}
		
		public override Value Perform (Value left)
		{
			return fac.Perform(left);
		}
	}
}

