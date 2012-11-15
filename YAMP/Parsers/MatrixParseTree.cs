using System;
using System.Collections.Generic;

namespace YAMP
{
	class MatrixParseTree : ParseTree
	{
		static Dictionary<string, Operator> operators = new Dictionary<string, Operator>();

		static internal void Register(Operator op)
		{
			operators.Add(op.Op, op);
		}

		public MatrixParseTree(QueryContext context, string input, int offset) : base(context, input, offset)
		{
		}

		protected override Value DefaultValue()
		{
			return new MatrixValue();
		}

		protected override Operator FindOperator(string input)
		{
			var op = Tokens.FindOperator(operators, Query, input);

			if (op != null)
				return op;

			op = Tokens.FindAvailableOperator(Query, input);

			if (op != null)
				return op;

			if(LastSkip == ' ' || LastSkip == '\t')
				return new MatrixColumnOperator();
			else if(LastSkip == '\n')
				return new MatrixRowOperator();

			throw new ParseException(input);
		}
	}
}
