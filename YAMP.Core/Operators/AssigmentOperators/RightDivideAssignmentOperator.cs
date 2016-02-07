namespace YAMP
{
    /// <summary>
    /// This is the class for the standard /= operator.
    /// </summary>
    class RightDivideAssignmentOperator : AssignmentPrefixOperator
    {
        public RightDivideAssignmentOperator() : 
            base(new RightDivideOperator())
        {
        }

        public override Operator Create()
        {
            return new RightDivideAssignmentOperator();
        }
    }
}
