using System;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// The standard right divide / operator.
    /// </summary>
	class RightDivideOperator : BinaryOperator
    {
        #region Mapping

        static List<BinaryOperatorMapping> mapping = new List<BinaryOperatorMapping>();

        public static List<BinaryOperatorMapping> Mapping
        {
            get { return mapping; }
        }

        static public void Register(Type a, Type b, Func<Value, Value, Value> f)
        {
            mapping.Add(new BinaryOperatorMapping(a, b, f));
        }

        #endregion

        #region ctor

        public RightDivideOperator () : base("/", 20)
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
            return new RightDivideOperator();
        }

        #endregion
    }
}

