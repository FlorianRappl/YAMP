using System;
using System.Text.RegularExpressions;

namespace YAMP
{
    class MatrixBracketExpression : BracketExpression
    {
        public MatrixBracketExpression() : base(@"\[.*\]", '[', ']')
        {
        }

        public MatrixBracketExpression(QueryContext query) : this()
        {
            DefaultOperator = ",";
            Query = query;
        }

        public override Expression Create(QueryContext query, Match match)
        {
            return new MatrixBracketExpression(query);
        }

        public override Value CreateDefaultValue()
        {
            return new MatrixValue();
        }
    }
}
