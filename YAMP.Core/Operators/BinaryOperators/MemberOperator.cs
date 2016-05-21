namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Exceptions;

    /// <summary>
    /// The member (dot) operator used to access members of objects.
    /// </summary>
    class MemberOperator : BinaryOperator
    {
        #region Mapping

        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList();
        public static readonly String Symbol = ".";

        #endregion

        #region ctor

        public MemberOperator()
            : base(Symbol, 10000)
		{
		}

        #endregion

        #region Methods

        public override Value Perform (Value left, Value right)
        {
            var obj = left as ObjectValue;

            if (obj == null)
            {
                throw new YAMPOperationInvalidException(Op, left);
            }

            if (right == null)
            {
                throw new YAMPOperationInvalidException(Op, right);
            }

            return obj.Perform(Context, right);
		}

        public override Value Handle(Expression left, Expression right, IDictionary<String, Value> symbols)
        {
            var l = left.Interpret(symbols);
            var symbol = right as SymbolExpression;
            var value = default(StringValue);
            
            if (symbol != null)
            {
                value = new StringValue(symbol.SymbolName);
            }
            
            return Perform(l, value);
        }

        public Value Handle(Expression left, Expression right, Value value, IDictionary<String, Value> symbols)
        {
            var obj = left.Interpret(symbols) as ObjectValue;
            var symbol = right as SymbolExpression;

            if (obj == null || symbol == null)
            {
                throw new YAMPOperationInvalidException(Op);
            }

            var key = new StringValue(symbol.SymbolName);
            return obj.Perform(Context, key, value);
        }

        public override Operator Create()
        {
            return new MemberOperator();
        }

        #endregion
    }
}
