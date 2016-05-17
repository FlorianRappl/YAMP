namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The standard right divide / operator.
    /// </summary>
	class RightDivideOperator : BinaryOperator
    {
        #region Mapping

        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList();

        public static void Register(Type a, Type b, Func<Value, Value, Value> f)
        {
            Mapping.With(new BinaryOperatorMapping(a, b, f));
        }

        #endregion

        #region ctor

        public RightDivideOperator () : 
            base("/", 20)
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
            return new RightDivideOperator();
        }

        #endregion
    }
}

