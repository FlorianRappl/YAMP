using System;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// This is the postfix increment operator ++.
    /// </summary>
    class PostIncOperator : RightUnaryOperator
    {
        public PostIncOperator()
            : base("++", 999)
        {
        }

        public override Value Handle(Expression expression, Dictionary<string, Value> symbols)
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

