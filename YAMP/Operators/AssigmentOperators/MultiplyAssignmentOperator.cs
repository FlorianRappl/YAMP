using System;

namespace YAMP
{
    class MultiplyAssignmentOperator : AssignmentPrefixOperator
    {
        public MultiplyAssignmentOperator() : base(new MultiplyOperator())
        {
        }

        public override Operator Create()
        {
            return new MultiplyAssignmentOperator();
        }
    }
}
