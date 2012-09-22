using System;

namespace YAMP
{
    class MinusAssignmentOperator : AssignmentPrefixOperator
    {
        public MinusAssignmentOperator() : base(new MinusOperator())
        {
        }

        public override Operator Create()
        {
            return new MinusAssignmentOperator();
        }
    }
}
