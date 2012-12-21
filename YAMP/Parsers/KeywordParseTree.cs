using System;

namespace YAMP
{
	class KeywordParseTree : StatementParseTree
	{
		#region ctor

		public KeywordParseTree(QueryContext context, string input, int line) : base(context, input, line)
		{
		}

		#endregion
	}
}
