using System;
using System.Collections;

namespace YAMP
{
	abstract class BinaryOperator : Operator
	{
		public BinaryOperator (string op, int level) : base(op, level)
		{
		}
		
		public abstract Value Perform(Value left, Value right);

        public virtual Value Handle(AbstractExpression left, AbstractExpression right, Hashtable symbols)
        {
            var l = left.Interpret(symbols);
            var r = right.Interpret(symbols);
            return Perform(l, r);
        }
		
		public override Value Evaluate (AbstractExpression[] expressions, Hashtable symbols)
		{
			if(expressions.Length != 2)
				throw new ArgumentsException(Op, expressions.Length);
			
			return Handle(expressions[0], expressions[1], symbols);
		}
	}
}

