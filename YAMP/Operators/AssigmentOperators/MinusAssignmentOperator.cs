using System;

namespace YAMP
{
    /// <summary>
    /// This is the class for the -= operator.
    /// </summary>
    class MinusAssignmentOperator : AssignmentPrefixOperator
    {
        public MinusAssignmentOperator() : base(new MinusOperator())
        {
        }

        public static MinusAssignmentOperator CreateWithContext(QueryContext context)
        {
            var a = new MinusAssignmentOperator();
            a.Query = context;
            return a;
        }

        public override Operator Create()
        {
            return new MinusAssignmentOperator();
        }
    }
}
