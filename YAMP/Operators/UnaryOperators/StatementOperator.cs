using System;

namespace YAMP
{
	class StatementOperator : UnaryOperator
	{
        QueryContext query;

		public StatementOperator(QueryContext query) : base(";", 1)
        {
            this.query = query;
        }

        public override Operator Create(QueryContext query)
        {
            return new StatementOperator(query);
        }

		public override Value Perform(Value value)
		{
			return value;
		}

		public override string Set(string input)
		{
			query.Statements.Rest = input.Substring(1);
			return string.Empty;
		}

		public override void RegisterElement()
		{
			//Do nothing.
		}
	}
}
