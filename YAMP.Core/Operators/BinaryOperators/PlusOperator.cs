namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The class for the standard + operator.
    /// </summary>
	class PlusOperator : BinaryOperator
    {
        #region Mapping

        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList();

        #endregion

        #region ctor

        static public void Register(Type a, Type b, Func<Value, Value, Value> f)
        {
            Mapping.With(new BinaryOperatorMapping(a, b, f));
        }

		public PlusOperator () : 
            base("+", 5)
		{
        }

        #endregion

        #region Methods
		
		public override Value Perform (Value left, Value right)
        {
            return PerformOverFind(left, right, Mapping);
		}

        public override Operator Create()
        {
            return new PlusOperator();
        }

        #endregion
    }
}

