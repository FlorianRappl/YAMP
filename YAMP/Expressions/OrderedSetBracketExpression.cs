using System;
using System.Text.RegularExpressions;

namespace YAMP
{
    class OrderedSetBracketExpression : BracketExpression
    {
        public OrderedSetBracketExpression() : base(@"\{.*\}", '{', '}')
        {
        }

        public OrderedSetBracketExpression(QueryContext query) : this()
        {
            Query = query;
        }

        public override Expression Create(QueryContext query, Match match)
        {
            return new OrderedSetBracketExpression(query);
        }

        public override Value CreateDefaultValue()
        {
            //TODO
            return Value.Empty;
        }
    }
}
