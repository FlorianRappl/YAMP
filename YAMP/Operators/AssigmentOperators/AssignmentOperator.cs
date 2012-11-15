using System;
using System.Collections;
using System.Collections.Generic;

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

		public override Operator Create(QueryContext query)
		{
			return new AssignmentOperator(query.Context);
		}

		public override Value Handle(Expression left, Expression right, Dictionary<string, Value> symbols)
		{
			var bottom = right.Interpret(symbols);
			return Assign(left, bottom, symbols);
		}
		
		public override Value Perform (Value left, Value right)
		{
			return right;
		}

		protected Value Assign(Expression left, Value value, Dictionary<string, Value> symbols)
		{
			if (left is SymbolExpression)
				return Assign(left as SymbolExpression, value);
			else if(left is TreeExpression)
			{
				var tree = left as TreeExpression;

				if (tree.Tree.Operator == null)
					return Assign(tree.Tree.Expressions[0], value, symbols);
				else if(tree.Tree.Operator is IndexOperator)
				{
					var ix = tree.Tree.Operator as IndexOperator;
					return ix.Handle(tree.Tree.Expressions[0], value, symbols);
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

