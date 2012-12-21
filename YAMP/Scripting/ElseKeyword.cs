using System;

namespace YAMP
{
	class ElseKeyword : Keyword
	{
		public ElseKeyword() : base(0)
		{
		}

		public override Keyword Create(QueryContext query)
		{
			throw new NotImplementedException();
		}
	}
}
