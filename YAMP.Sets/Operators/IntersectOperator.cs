namespace YAMP.Sets
{
    using System;

    /// <summary>
    /// The Intersect operator.
    /// </summary>
    class IntersectOperator : BinaryOperator
    {
        #region Mapping

        public static readonly String Symbol = OpDefinitionsSet.IntersectOperator;
        public static readonly int OpLevel = OpDefinitionsSet.IntersectOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        public IntersectOperator()
            : base(Symbol, OpLevel)
		{
		}

        public override Value Perform(Value left, Value right)
        {
            return PerformOverFind(left, right, Mapping);
        }

        public override Operator Create()
        {
            return new IntersectOperator();
        }
    }
}
