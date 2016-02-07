namespace YAMP
{
    /// <summary>
    /// This is the class for the -= operator.
    /// </summary>
    class MinusAssignmentOperator : AssignmentPrefixOperator
    {
        public MinusAssignmentOperator() : 
            base(new MinusOperator())
        {
        }

        public static MinusAssignmentOperator CreateWithContext(QueryContext context)
        {
            return new MinusAssignmentOperator
            {
                Query = context
            };
        }

        public override Operator Create()
        {
            return new MinusAssignmentOperator();
        }
    }
}
