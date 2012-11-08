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

        public override Value CreateDefaultValue()
        {
            return new ArgumentsValue();
        }
    }
}
