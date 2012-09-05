using System;

namespace YAMP
{
	class FacultyOperator : Operator
	{
		static FacultyFunction fac = new FacultyFunction();
		
		public FacultyOperator () : base("!", 200)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			return fac.Perform(left);
		}
		
		public override string Set (string input)
		{
			return "0" + base.Set (input);
		}
	}
}

