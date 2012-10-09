using System;

namespace YAMP
{
    class LeftDivideAssignmentOperator : AssignmentPrefixOperator
    {
        public LeftDivideAssignmentOperator(ParseContext context) : base(context, new LeftDivideOperator())
        {
        }

        public LeftDivideAssignmentOperator() : this(ParseContext.Default)
        {
        }

        public override Operator Create(ParseContext context)
        {
            return new LeftDivideAssignmentOperator(context);
        }
    }
}
