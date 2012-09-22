using System;

namespace YAMP
{
    class LeftDivideAssignmentOperator : AssignmentPrefixOperator
    {
        public LeftDivideAssignmentOperator() : base(new LeftDivideOperator())
        {
        }

        public override Operator Create()
        {
            return new LeftDivideAssignmentOperator();
        }
    }
}
