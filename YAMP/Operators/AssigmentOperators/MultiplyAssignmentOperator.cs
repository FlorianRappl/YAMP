using System;

namespace YAMP
{
    class MultiplyAssignmentOperator : AssignmentPrefixOperator
    {
        public MultiplyAssignmentOperator(ParseContext context) : base(context, new MultiplyOperator())
        {
        }

        public MultiplyAssignmentOperator() : this(ParseContext.Default)
        {
        }

        public override Operator Create(QueryContext query)
        {
            return new MultiplyAssignmentOperator(query.Context);
        }
    }
}
