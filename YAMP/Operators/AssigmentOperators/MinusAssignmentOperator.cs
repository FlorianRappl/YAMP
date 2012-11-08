using System;

namespace YAMP
{
    class MinusAssignmentOperator : AssignmentPrefixOperator
    {
        public MinusAssignmentOperator(ParseContext context) : base(context, new MinusOperator())
        {
        }

        public MinusAssignmentOperator() : this(ParseContext.Default)
        {
        }

        public override Operator Create(QueryContext query)
        {
            return new MinusAssignmentOperator(query.Context);
        }
    }
}
