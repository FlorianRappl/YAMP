using System;

namespace YAMP
{
    /// <summary>
    /// The construction scheme for a greater than operator.
    /// </summary>
	class GtOperator : LogicOperator
	{
		public GtOperator () : base(">")
		{
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
            return new ScalarValue(left > right);
		}

        public override Operator Create()
        {
            return new GtOperator();
        }
	}
}

