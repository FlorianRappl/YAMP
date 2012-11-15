using System;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
	public abstract class BinaryOperator : Operator
	{
		public BinaryOperator(string op, int level) : base(op, level)
		{
		}
		
		public abstract Value Perform(Value left, Value right);

		public virtual Value Handle(Expression left, Expression right, Dictionary<string, Value> symbols)
		{
			var l = left.Interpret(symbols);
			var r = right.Interpret(symbols);
			return Perform(l, r);
		}

		public override Value Evaluate(Expression[] expressions, Dictionary<string, Value> symbols)
		{
			if (expressions.Length != 2)
				throw new ArgumentsException(Op, 2);

			return Handle(expressions[0], expressions[1], symbols);
		}
	}
}