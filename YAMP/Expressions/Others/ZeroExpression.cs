using System;
using System.Collections;

namespace YAMP
{
	class ZeroExpression : AbstractExpression
	{
		Value _value;
		
		public ZeroExpression () : this(new ScalarValue())
		{
		}
		
		public ZeroExpression (Value value) : base(@" ")
		{
			_value = value;
		}
		
		public override string Set (string input)
		{
			return input;
		}
		
		public override Value Interpret (Hashtable symbols)
		{
			return _value;
		}
	}
}

