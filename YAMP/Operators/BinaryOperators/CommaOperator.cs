using System;
using System.Collections.Generic;

namespace YAMP
{
	class CommaOperator : BinaryOperator
	{
        static protected List<CommaOperator> operators = new List<CommaOperator>();

		public CommaOperator () : base(",", 2)
		{
		}

        public override Value Perform(Value left, Value right)
        {
            return left;
        }

        public override Operator Create(ParseContext context, Expression premise)
        {
            foreach (var op in operators)
            {
                if (op.Dependency.IsInstanceOfType(premise))
                    return op.Create(context);
            }

            throw new OperationNotSupportedException(Op, "in this context");
        }
	}
}

