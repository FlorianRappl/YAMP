using System;

namespace YAMP
{
	class FacultyOperator : Operator
	{
		public FacultyOperator () : base("!", 200)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			return left.Faculty();
		}
		
		public override string Set (string input)
		{
			return "0" + base.Set (input);
		}
	}
}

