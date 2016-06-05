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

        public static readonly String Symbol = OpDefinitions.MemberOperator;
        public static readonly int OpLevel = OpDefinitions.MemberOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        #region ctor

        public MemberOperator()
            : base(Symbol, OpLevel)
        {
		}

        #endregion

        #region Methods

        public override Value Perform(Value left, Value right)
        {
            return PerformOverFind(left, right, Mapping);
		}

        public override Value Handle(Expression left, Expression right, IDictionary<String, Value> symbols)
        {
            var l = left.Interpret(symbols);
            var symbol = right as SymbolExpression;
            var value = default(StringValue);

            //Is it a Method Call?
            if (symbol == null)
            {
                var contExp = right as ContainerExpression;
                if (contExp != null)
                {
                    if (contExp.Expressions != null && contExp.Expressions.Length == 1 && contExp.Expressions[0] is SymbolExpression)
                    {
                        symbol = contExp.Expressions[0] as SymbolExpression;
                        var op = contExp.Operator as ArgsOperator;
                        if (op != null)
                        {
                            return op.Handle(symbol, symbols, l);
                        }
                        //YAMPMemberFunctionMissingException
                    }
                }
            }
            
            if (symbol != null)
            {
                value = new StringValue(symbol.SymbolName);
            }

            if (value == null)
                throw new YAMPFunctionMissingException("()");
            
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
