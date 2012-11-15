using System;

namespace YAMP
{
	class StatementOperator : UnaryOperator
	{
		public StatementOperator() : base(";", 1)
		{
		}

		public override Value Perform(Value value)
		{
			return value;
		}

		public override string Set(string input)
		{
			return string.Empty;
		}

		public override void RegisterToken()
		{
			//Do nothing.
		}
	}
}
