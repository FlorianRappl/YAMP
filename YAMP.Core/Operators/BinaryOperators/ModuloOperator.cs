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

        static List<BinaryOperatorMapping> mapping = new List<BinaryOperatorMapping>();

        public static List<BinaryOperatorMapping> Mapping
        {
            get { return mapping; }
        }

        public static void Register(Type a, Type b, Func<Value, Value, Value> f)
        {
            mapping.Add(new BinaryOperatorMapping(a, b, f));
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
            return PerformOverFind(left, right, mapping);
		}

        public override Operator Create()
        {
            return new ModuloOperator();
        }

        #endregion
    }
}
