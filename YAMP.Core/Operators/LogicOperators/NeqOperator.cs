namespace YAMP
{
    using System;

    /// <summary>
    /// This is the not equal to operator.
    /// </summary>
	abstract class NeqOperator : LogicOperator
	{
        public static readonly int OpLevel = OpDefinitions.NeqOperatorLevel;

        public NeqOperator(String op)
            : base(op, OpLevel)
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
            #region Mapping

            public static readonly String Symbol = OpDefinitions.StandardNeqOperator;
            public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

            #endregion

            public StandardNeqOperator()
                : base(Symbol)
            {
            }

            public override Value Perform(Value left, Value right)
            {
                Boolean found;
                Value ret = TryPerformOverFind(left, right, Mapping, out found);
                if (!found)
                {
                    if (left is StringValue || right is StringValue)
                    {
                        return new ScalarValue(left.ToString(Context) != right.ToString(Context));
                    }

                    return base.Perform(left, right);
                }
                return ret;
            }

            public override Operator Create()
            {
                return new StandardNeqOperator();
            }
        }

        public class AliasNeqOperator : NeqOperator
        {
            #region Mapping

            public static readonly String Symbol = OpDefinitions.AliasNeqOperator;
            public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

            #endregion

            public AliasNeqOperator()
                : base(Symbol)
            {
            }

            public override Value Perform(Value left, Value right)
            {
                Boolean found;
                Value ret = TryPerformOverFind(left, right, Mapping, out found);
                if (!found)
                {
                    if (left is StringValue || right is StringValue)
                    {
                        return new ScalarValue(left.ToString(Context) != right.ToString(Context));
                    }

                    return base.Perform(left, right);
                }
                return ret;
            }

            public override Operator Create()
            {
                return new AliasNeqOperator();
            }
        }
	}
}

