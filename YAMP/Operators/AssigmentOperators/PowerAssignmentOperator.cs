using System;

namespace YAMP
{
    class PowerAssignmentOperator : AssignmentPrefixOperator
    {
        public PowerAssignmentOperator(ParseContext context)
            : base(context, new PowerOperator())
        {
        }

        public PowerAssignmentOperator()
            : this(ParseContext.Default)
        {
        }

        public override Operator Create(ParseContext context)
        {
            return new PowerAssignmentOperator(context);
        }
    }
}
