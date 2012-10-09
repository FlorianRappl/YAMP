using System;
using System.Collections;

namespace YAMP
{
	class AssignmentOperator : BinaryOperator
	{
        protected ParseContext _context;

		public AssignmentOperator () : this(ParseContext.Default, string.Empty)
		{
		}

        public AssignmentOperator(ParseContext context, string prefix) : base(prefix + "=", -1)
        {
            _context = context;
        }

        public AssignmentOperator(ParseContext context) : this(context, string.Empty)
        {
        }

        public override Operator Create()
        {
            return Create(ParseContext.Default);
        }

        public override Operator Create(ParseContext context)
        {
            return new AssignmentOperator(context);
        }

        public override Value Handle(Expression left, Expression right, Hashtable symbols)
        {
            var bottom = right.Interpret(symbols);
			return Assign(left, bottom, symbols);
        }
		
		public override Value Perform (Value left, Value right)
		{
			return right;
		}
		
		protected Value Assign(Expression left, Value value, Hashtable symbols)
		{
			if (left is SymbolExpression)
                return Assign(left as SymbolExpression, value);
			else if(left is BracketExpression)
			{
				var bracket = left as BracketExpression;
				
				if(bracket.Tree.Operator == null)
					return Assign(bracket.Tree.Expressions[0], value, symbols);
				else if(bracket.Tree.Operator is IndexOperator)
				{
					var ix = bracket.Tree.Operator as IndexOperator;
					return ix.Handle(bracket.Tree.Expressions[0], value, symbols);
				}
				else
					throw new AssignmentException(Op);
			}
			
			return value;
		}

        Value Assign(SymbolExpression left, Value value)
		{
            if(left.IsSymbol)
                _context.AssignVariable(left.SymbolName, value);
			else
				throw new AssignmentException(Op);
			
			return value;
		}
	}
}

