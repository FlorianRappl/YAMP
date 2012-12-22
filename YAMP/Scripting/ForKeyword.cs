using System;
using System.Collections.Generic;

namespace YAMP
{
    class ForKeyword : BreakableKeyword
    {
        #region ctor

        public ForKeyword() : base("for", 3)
        {
        }

		public ForKeyword(QueryContext query) : this()
		{
			Query = query;
		}

        #endregion

        #region Methods

        public override Keyword Create(QueryContext query)
        {
            return new ForKeyword(query);
        }

        public override Value Run(Dictionary<string, Value> symbols)
        {
            Value rtrn = null;
            Initialize(symbols);

            while (Condition(symbols))
            {
                rtrn = Body.Interpret(symbols);

                if (Break)
                    break;

                Finalize(symbols);
            }

            return rtrn;
        }

        void Initialize(Dictionary<string, Value> symbols)
        {
            Arguments[0].Interpret(symbols);
        }

        bool Condition(Dictionary<string, Value> symbols)
        {
            var condition = Arguments[1].Interpret(symbols);

            if (condition != null && condition is ScalarValue)
                return ((ScalarValue)condition).IsTrue;

            throw new ParseException(Offset, Token + ". A boolean value is required to determine the condition");
        }

        void Finalize(Dictionary<string, Value> symbols)
        {
            Arguments[2].Interpret(symbols);
        }

        #endregion
    }
}
