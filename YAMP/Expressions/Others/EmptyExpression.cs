using System;
using System.Collections;

namespace YAMP
{
	class EmptyExpression : AbstractExpression
	{
		public EmptyExpression () : base("")
		{
		}
		
		public override string Set (string input)
		{
			return input;
		}
		
		public override void RegisterToken ()
		{
			//Left blank intentionally.
		}
		
		public override Value Interpret (Hashtable symbols)
		{
			return new MatrixValue();
		}
	}
}

