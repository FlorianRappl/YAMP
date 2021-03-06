﻿namespace YAMP
{
    /// <summary>
    /// This class represents the ^= operator.
    /// </summary>
    class PowerAssignmentOperator : AssignmentPrefixOperator
    {
        public PowerAssignmentOperator() : 
            base(new PowerOperator())
        {
        }

        public override Operator Create()
        {
            return new PowerAssignmentOperator();
        }
    }
}
