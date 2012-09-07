using System;
using System.Collections;

namespace YAMP
{
	abstract class UnaryOperator : Operator
	{
		public UnaryOperator (string op) : base(op)
		{
		}
		
		public abstract Value Perform(Value value);

        public virtual Value Handle(AbstractExpression expression, Hashtable symbols)
        {
            var value = expression.Interpret(symbols);
            return Perform(value);
        }
		
		public override Value Evaluate (AbstractExpression[] expressions, Hashtable symbols)
		{
			if(expressions.Length != 1)
				throw new ArgumentsException(Op, expressions.Length);
			
			return Handle(expressions[0], symbols);
		}
	}
}

