using System;
using System.Collections.Generic;

namespace YAMP
{
	class RootParseTree : ParseTree
	{
		#region Members

		int line;
		RootParseTree child;

		#endregion

		#region ctor

		public RootParseTree(QueryContext query, string input) : base(query, input)
		{
		}

		public RootParseTree(QueryContext query, string input, int line) : base(query, input)
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

		/// <summary>
		/// Gets a value if the interpreter has any successor that should be interpreted afterwards.
		/// </summary>
		public bool HasSuccessor
		{
			get
			{
				return child != null;
			}
		}

		#endregion

		#region Methods

		protected override Operator FindOperator(string input)
		{
			if (input[0] == ';')
			{
				var tree = new RootParseTree(Query, input.Substring(1), line + 1);

				if (tree.HasContent)
					child = tree;
				else
					Query.IsMuted = true;
				
				return new StatementOperator();
			}

			return Tokens.Instance.FindOperator(Query, input);
		}

		internal override Value Interpret(Dictionary<string, Value> symbols)
		{
			var self = base.Interpret(symbols);

			if (HasSuccessor)
				return child.Interpret(symbols);

			return self;
		}

		#endregion
	}
}
