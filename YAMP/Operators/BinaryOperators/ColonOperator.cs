using System;
using System.Collections.Generic;

namespace YAMP
{
	class ColonOperator : BinaryOperator
    {
        static protected List<ColonOperator> operators = new List<ColonOperator>();

		public ColonOperator () : base(";", 1)
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