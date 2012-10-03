using System;
using System.Collections;

namespace YAMP
{
    public abstract class UnaryOperator : Operator
	{
		public UnaryOperator (string op, int level) : base(op, level, false)
		{
		}
		
		public abstract Value Perform(Value value);

        public virtual Value Handle(Expression expression, Hashtable symbols)
        {
            var value = expression.Interpret(symbols);
            return Perform(value);
        }
		
		public override Value Evaluate (Expression[] expressions, Hashtable symbols)
		{
			if(expressions.Length != 1)
				throw new ArgumentsException(Op, expressions.Length);
			
			return Handle(expressions[0], symbols);
		}
	}
}

