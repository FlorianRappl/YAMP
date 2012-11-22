using System;
using System.Collections.Generic;

namespace YAMP
{
	class StatementParseTree : ParseTree
	{
		#region Members

		int line;

		#endregion

		#region ctor

		public StatementParseTree(QueryContext query, string input, int line) : base(query, input)
		{
			this.line = line;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the statement line of this expression.
		/// </summary>
		public int Line
		{
			get
			{
				return line;
			}
		}

		#endregion

		#region Methods

		protected override Operator FindOperator(string input)
		{
            if (input[0] == ';')
            {
                Input = Input.Remove(Input.Length - input.Length);
                return new StatementOperator(Query);
            }

			return Tokens.Instance.FindOperator(Query, input);
		}

        public override string ToString()
        {
            return "-> " + Input;
        }

		#endregion
	}
}
