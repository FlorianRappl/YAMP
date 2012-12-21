using System;

namespace YAMP
{
	class IfKeyword : Keyword
	{
		public IfKeyword() : base(1)
		{
		}

		public IfKeyword(QueryContext query) : this()
		{
			Query = query;
		}

		public override Keyword Create(QueryContext query)
		{
			return new IfKeyword(query);
		}
	}
}
