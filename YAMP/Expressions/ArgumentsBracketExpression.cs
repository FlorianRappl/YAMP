using System;
using System.Text.RegularExpressions;

namespace YAMP
{
    class ArgumentsBracketExpression : BracketExpression
    {
        public ArgumentsBracketExpression() : this(ParseContext.Default)
        {
        }

        public ArgumentsBracketExpression(ParseContext context) : base(@"\(.*\)", '(', ')')
        {
            Context = context;
        }

        public override Expression Create(ParseContext context, Match match)
        {
            return new ArgumentsBracketExpression(context);
        }

        public override Value CreateDefaultValue()
        {
            return new ArgumentsValue();
        }
    }
}
