using System;
using System.Collections;

namespace YAMP
{
    class PlusAssignmentOperator : AssignmentPrefixOperator
    {
        public PlusAssignmentOperator() : base(new PlusOperator())
        {
        }

        public override Operator Create()
        {
            return new PlusAssignmentOperator();
        }
    }
}
