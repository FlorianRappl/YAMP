using System;

namespace YAMP
{
	class TerminateOperator : UnaryOperator
	{
		QueryContext query;

		public TerminateOperator(QueryContext query) : base(";", 1)
		{
			this.query = query;
		}

		public override Operator Create(QueryContext query)
		{
			return new TerminateOperator(query);
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
