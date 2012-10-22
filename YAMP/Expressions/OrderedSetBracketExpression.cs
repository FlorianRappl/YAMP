using System;
using System.Text.RegularExpressions;

namespace YAMP
{
    class OrderedSetBracketExpression : BracketExpression
    {
        public OrderedSetBracketExpression() : this(ParseContext.Default)
        {
        }

        public OrderedSetBracketExpression(ParseContext context) : base(@"\{.*\}", '{', '}')
        {
            Context = context;
        }

        public override Expression Create(ParseContext context, Match match)
        {
            return new OrderedSetBracketExpression(context);
        }

        public override Value CreateDefaultValue()
        {
            //TODO
            return Value.Empty;
        }
    }
}
