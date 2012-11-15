using System;
using System.Collections.Generic;

namespace YAMP
{
	class ArgumentsParseTree : ParseTree
	{
		static Dictionary<string, Operator> operators = new Dictionary<string, Operator>();

		static internal void Register(Operator op)
		{
			operators.Add(op.Op, op);
		}

		public ArgumentsParseTree(QueryContext context, string input, int offset) : base(context, input, offset)
		{
		}

		protected override Value DefaultValue()
		{
			return new ArgumentsValue();
		}

		protected override Operator FindOperator(string input)
		{
			var op = Tokens.FindOperator(operators, Query, input);

			if (op != null)
				return op;

			return Tokens.Instance.FindOperator(Query, input);
		}
	}
}
