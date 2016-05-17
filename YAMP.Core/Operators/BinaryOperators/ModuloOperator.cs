namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The class for the standard modulo operator.
    /// </summary>
	class ModuloOperator : BinaryOperator
    {
        #region Mapping

        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList();

        public static void Register(Type a, Type b, Func<Value, Value, Value> f)
        {
            Mapping.With(new BinaryOperatorMapping(a, b, f));
        }

        #endregion

        #region ctor

        public ModuloOperator() : 
            base("%", 30)
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
            return new ModuloOperator();
        }

        #endregion
    }
}
