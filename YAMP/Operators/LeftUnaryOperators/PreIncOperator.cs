using System;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// This is the prefix increment operator ++.
    /// </summary>
    class PreIncOperator : LeftUnaryOperator
    {
        static readonly PlusAssignmentOperator assignment = new PlusAssignmentOperator();

        public PreIncOperator() : base("++", 999)
        {
        }

        public override Value Handle(Expression value, Dictionary<string, Value> symbols)
        {
            var a = PlusAssignmentOperator.CreateWithContext(Query);
            a.Handle(value, new NumberExpression(ScalarValue.One), symbols);
            return value.Interpret(symbols);
        }

        public override Operator Create()
        {
            return new PreIncOperator();
        }
    }
}

