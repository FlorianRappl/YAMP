using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YAMP
{
	class MatrixBracketExpression : BracketExpression
	{
		public MatrixBracketExpression() : base(@"\[.*\]", '[', ']')
		{
		}

		public MatrixBracketExpression(QueryContext query) : this()
		{
			Query = query;
		}

		public override Expression Create(QueryContext query, Match match)
		{
			return new MatrixBracketExpression(query);
		}

		protected override ParseTree CreateParseTree(string input)
		{
			return new MatrixParseTree(Query, input, Offset);
		}
	}
}
