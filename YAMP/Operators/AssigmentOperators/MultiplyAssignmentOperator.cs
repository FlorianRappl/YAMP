using System;

namespace YAMP
{
    /// <summary>
    /// This is the class representing the *= operator.
    /// </summary>
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
