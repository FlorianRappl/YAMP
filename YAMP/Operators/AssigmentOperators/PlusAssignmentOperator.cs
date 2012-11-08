using System;
using System.Collections;

namespace YAMP
{
    class PlusAssignmentOperator : AssignmentPrefixOperator
    {
        public PlusAssignmentOperator(ParseContext context) : base(context, new PlusOperator())
        {
        }

        public PlusAssignmentOperator() : this(ParseContext.Default)
        {
        }

        public override Operator Create(QueryContext query)
        {
            return new PlusAssignmentOperator(query.Context);
        }
    }
}
