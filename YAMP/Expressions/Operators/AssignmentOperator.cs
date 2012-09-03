using System;
using System.Collections;

namespace YAMP
{
	class AssignmentOperator : Operator
	{
		public AssignmentOperator () : this(string.Empty)
		{
		}

        public AssignmentOperator(string prefix) : base(prefix + "=", 0)
        {
        }

        public override Value Handle(AbstractExpression left, AbstractExpression right, Hashtable symbols)
        {
            var bottom = right.Interpret(symbols);

            if (left is SymbolExpression)
                Assign(left as SymbolExpression, bottom);

            return bottom;
        }
		
		public override Value Perform (Value left, Value right)
		{
			return right;
		}

        void Assign(SymbolExpression left, Value value)
		{
            if(left.IsSymbol)
                Tokens.Instance.AssignVariable(left.SymbolName, value);
		}
	}
}

