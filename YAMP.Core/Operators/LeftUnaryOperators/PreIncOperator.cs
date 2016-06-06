namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the prefix increment operator ++.
    /// </summary>
    class PreIncOperator : LeftUnaryOperator
    {
        public static readonly String Symbol = OpDefinitions.PreIncOperator;
        public static readonly int OpLevel = OpDefinitions.PreIncOperatorLevel;

        static readonly PlusAssignmentOperator assignment = new PlusAssignmentOperator();

        public PreIncOperator() : base(Symbol, OpLevel)
        {
        }

        public override Value Handle(Expression value, IDictionary<String, Value> symbols)
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

