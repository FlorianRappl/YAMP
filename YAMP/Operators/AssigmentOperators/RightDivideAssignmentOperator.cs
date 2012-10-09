using System;

namespace YAMP
{
    class RightDivideAssignmentOperator : AssignmentPrefixOperator
    {
        public RightDivideAssignmentOperator(ParseContext context)
            : base(context, new RightDivideOperator())
        {
        }

        public RightDivideAssignmentOperator() : this(ParseContext.Default)
        {
        }

        public override Operator Create(ParseContext context)
        {
            return new RightDivideAssignmentOperator(context);
        }
    }
}
