using System;
using System.Collections;

namespace YAMP
{
    /// <summary>
    /// This class represents the += operator.
    /// </summary>
    class PlusAssignmentOperator : AssignmentPrefixOperator
    {
        public PlusAssignmentOperator() : base(new PlusOperator())
        {
        }

        public static PlusAssignmentOperator CreateWithContext(QueryContext context)
        {
            var a = new PlusAssignmentOperator();
            a.Query = context;
            return a;
        }

        public override Operator Create()
        {
            return new PlusAssignmentOperator();
        }
    }
}
