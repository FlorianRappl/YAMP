using System;

namespace YAMP
{
	class StatementOperator : UnaryOperator
	{
        QueryContext query;

		public StatementOperator() : base(";", 1)
		{
		}

        public StatementOperator(QueryContext query) : this()
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
            query.Statements.AddStatement(input.Substring(1));
			return string.Empty;
		}

		public override void RegisterToken()
		{
			//Do nothing.
		}
	}
}
