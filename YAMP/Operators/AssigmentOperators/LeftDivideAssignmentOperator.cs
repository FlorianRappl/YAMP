using System;

namespace YAMP
{
    /// <summary>
    /// This is the class for the \= operator.
    /// </summary>
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
