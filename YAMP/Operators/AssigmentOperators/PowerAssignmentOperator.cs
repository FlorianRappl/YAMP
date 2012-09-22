using System;

namespace YAMP
{
    class PowerAssignmentOperator : AssignmentPrefixOperator
    {
        public PowerAssignmentOperator() : base(new PowerOperator())
        {
        }

        public override Operator Create()
        {
            return new PowerAssignmentOperator();
        }
    }
}
