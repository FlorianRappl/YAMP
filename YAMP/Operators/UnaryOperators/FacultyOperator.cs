using System;

namespace YAMP
{
	class FacultyOperator : UnaryOperator
	{
		static readonly FacultyFunction fac = new FacultyFunction();
		
		public FacultyOperator () : base("!", 1000)
		{
		}

        public override Operator Create()
        {
            return new FacultyOperator();
        }
		
		public override Value Perform (Value left)
		{
			return fac.Perform(left);
		}
	}
}

