using System;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    public abstract class UnaryOperator : Operator
	{
		public UnaryOperator (string op, int level) : base(op, level, false)
		{
		}
		
		public abstract Value Perform(Value value);

        public virtual Value Handle(Expression expression, Dictionary<string, object> symbols)
        {
            var value = expression.Interpret(symbols);
            return Perform(value);
        }
		
		public override Value Evaluate(Expression[] expressions, Dictionary<string, object> symbols)
        {
            if (expressions.Length != 1)
                throw new ArgumentsException(Op, 1);

			return Handle(expressions[0], symbols);
		}
	}
}

