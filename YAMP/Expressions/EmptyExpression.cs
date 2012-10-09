using System;
using System.Text.RegularExpressions;
using System.Collections;

namespace YAMP
{
	class EmptyExpression : Expression
	{
		public EmptyExpression () : base("")
		{
		}

        public override Expression Create(ParseContext context, Match match)
        {
			return new EmptyExpression();
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
			return new ArgumentsValue();
		}
	}
}

