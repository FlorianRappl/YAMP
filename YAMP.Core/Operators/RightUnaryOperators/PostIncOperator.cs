namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the postfix increment operator ++.
    /// </summary>
    class PostIncOperator : RightUnaryOperator
    {
        public static readonly String Symbol = OpDefinitions.PostIncOperator;
        public static readonly int OpLevel = OpDefinitions.PostIncOperatorLevel;

        public PostIncOperator()
            : base(Symbol, OpLevel)
        {
        }

        public override Value Handle(Expression expression, IDictionary<String, Value> symbols)
        {
            var a = PlusAssignmentOperator.CreateWithContext(Query);
            var origin = expression.Interpret(symbols);
            a.Handle(expression, new NumberExpression(ScalarValue.One), symbols);
            return origin;
        }

        public override Operator Create()
        {
            return new PostIncOperator();
        }
    }
}

