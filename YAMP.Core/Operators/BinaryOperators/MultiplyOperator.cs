namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The class for the standard multiply operator.
    /// </summary>
	class MultiplyOperator : BinaryOperator
    {
        #region Mapping

        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList();

        public static void Register(Type a, Type b, Func<Value, Value, Value> f)
        {
            Mapping.With(new BinaryOperatorMapping(a, b, f));
        }

        #endregion

        #region ctor

        public MultiplyOperator () :
            base("*", 10)
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
            return new MultiplyOperator();
        }

        #endregion
    }
}

