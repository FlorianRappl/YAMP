using System;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// This is the prefix decrement operator --.
    /// </summary>
    class PreDecOperator : LeftUnaryOperator
    {
        public PreDecOperator()
            : base("--", 999)
        {
        }

        public override Value Handle(Expression value, Dictionary<string, Value> symbols)
        {
            var a = MinusAssignmentOperator.CreateWithContext(Query);
            a.Handle(value, new NumberExpression(ScalarValue.One), symbols);
            return value.Interpret(symbols);
        }

        public override Operator Create()
        {
            return new PreDecOperator();
        }
    }
}

