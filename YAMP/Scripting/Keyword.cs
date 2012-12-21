using System;

namespace YAMP
{
	abstract class Keyword : IRegisterElement
	{
		#region ctor

		public Keyword(int arguments)
		{
			Arguments = arguments;
			Token = GetType().Name.Replace("Keyword", string.Empty).ToLower();
		}

		#endregion

		#region Properties

		public string Token { get; private set; }

		public int Arguments { get; private set; }

		public QueryContext Query { get; protected set; }

		#endregion

		#region Methods

		public void RegisterElement()
		{
			Elements.Instance.AddKeyword(Token, this);
		}

		public abstract Keyword Create(QueryContext query);

		#endregion
	}
}
