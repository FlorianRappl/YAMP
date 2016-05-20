namespace YAMP
{
    using System;

    /// <summary>
    /// This is the not equal to operator.
    /// </summary>
	abstract class NeqOperator : LogicOperator
	{
		public NeqOperator(String op)
            : base(op)
		{
		}

		public override ScalarValue Compare(ScalarValue l, ScalarValue r)
		{
            return new ScalarValue(l != r);
		}

        public override Value Perform(Value left, Value right)
        {
            if (left is StringValue || right is StringValue)
            {
                return new ScalarValue(left.ToString(Context) != right.ToString(Context));
            }

            return base.Perform(left, right);
        }

        public class StandardNeqOperator : NeqOperator
        {
            public StandardNeqOperator()
                : base("~=")
            {
            }

            public override Operator Create()
            {
                return new StandardNeqOperator();
            }
        }

        public class AliasNeqOperator : NeqOperator
        {
            public AliasNeqOperator()
                : base("!=")
            {
            }

            public override Operator Create()
            {
                return new AliasNeqOperator();
            }
        }
	}
}

