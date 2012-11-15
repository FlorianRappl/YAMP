using System;
using System.Text.RegularExpressions;

namespace YAMP
{
	class ArgumentsBracketExpression : BracketExpression
	{
		public ArgumentsBracketExpression() : base(@"\(.*\)", '(', ')')
		{
		}

		public ArgumentsBracketExpression(QueryContext query) : this()
		{
			Query = query;
		}

		public override Expression Create(QueryContext query, Match match)
		{
			return new ArgumentsBracketExpression(query);
		}

		protected override ParseTree CreateParseTree(string input)
		{
			return new ArgumentsParseTree(Query, input, Offset);
		}
	}
}
