using System;

namespace YAMP
{
    /// <summary>
    /// This is the lighter than operator.
    /// </summary>
	class LtOperator : LogicOperator
	{
		public LtOperator () : base("<")
		{
		}

		public override ScalarValue Compare(ScalarValue left, ScalarValue right)
		{
            return new ScalarValue(left < right);
		}

        public override Operator Create()
        {
            return new LtOperator();
        }
	}
}

