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
				else if(tree.Tree.Operator is ArgsOperator)
				{
					var ix = (ArgsOperator)tree.Tree.Operator;
					return ix.Handle(tree.Tree.Expressions[0], value, symbols);
				}
                else if (tree.Tree.IsSymbolList)
                {
                    var vars = GetSymbols(tree.Tree);
                    return HandleMultipleOutputs(value, vars);
                }
                else
                    throw new AssignmentException(Op);
			}
			
			return value;
		}

        Value HandleMultipleOutputs(Value value, SymbolExpression[] vars)
        {

            if (value is ArgumentsValue)
            {
                var av = (ArgumentsValue)value;
                var l = Math.Min(vars.Length, av.Length);

                for (var i = 0; i != l; i++)
                    Assign(vars[i], av.Values[i]);

                return av;
            }
            else
            {
                foreach (var sym in vars)
                    Assign(sym, value);

                return value;
            }
        }

        SymbolExpression[] GetSymbols(ParseTree tree)
        {
            var list = new List<SymbolExpression>();

            foreach (var expression in tree.Expressions)
            {
                if (expression is TreeExpression)
                    list.AddRange(GetSymbols(((TreeExpression)expression).Tree));
                else if (expression is SymbolExpression)
                    list.Add((SymbolExpression)expression);
            }

            return list.ToArray();
        }

		Value Assign(SymbolExpression left, Value value)
		{
			_context.AssignVariable(left.SymbolName, value);
			return value;
		}
	}
}

