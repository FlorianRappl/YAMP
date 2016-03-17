namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the postfix decrement operator --.
    /// </summary>
    class PostDecOperator : RightUnaryOperator
    {
        public PostDecOperator()
            : base("--", 999)
        {
        }

        public override Value Handle(Expression expression, IDictionary<String, Value> symbols)
        {
            var a = MinusAssignmentOperator.CreateWithContext(Query);
            var origin = expression.Interpret(symbols);
            a.Handle(expression, new NumberExpression(ScalarValue.One), symbols);
            return origin;
        }

        public override Operator Create()
        {
            return new PostDecOperator();
        }
    }
}

