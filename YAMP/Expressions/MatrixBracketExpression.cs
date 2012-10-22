using System;
using System.Text.RegularExpressions;

namespace YAMP
{
    class MatrixBracketExpression : BracketExpression
    {
        public MatrixBracketExpression() : this(ParseContext.Default)
        {
        }

        public MatrixBracketExpression(ParseContext context) : base(@"\[.*\]", '[', ']')
        {
            DefaultOperator = ",";
            Context = context;
        }

        public override Expression Create(ParseContext context, Match match)
        {
            return new MatrixBracketExpression(context);
        }

        public override Value CreateDefaultValue()
        {
            return new MatrixValue();
        }
    }
}
