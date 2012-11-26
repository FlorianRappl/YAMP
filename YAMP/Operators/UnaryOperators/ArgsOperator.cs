using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    class ArgsOperator : UnaryOperator
	{
		#region Members

		ArgumentsBracketExpression _content;
        QueryContext _query;

		#endregion

		#region ctor

		public ArgsOperator() : base("(", 1000)
        {
        }

        public ArgsOperator(QueryContext query)
            : base("(", 1000)
        {
            _query = query;
		}

		#endregion

		#region Properties

		public override string Input
		{
			get
			{
				return _content.Input;
			}
		}

		#endregion

		#region Methods

		public override Operator Create(QueryContext query)
        {
            return new ArgsOperator(query);
        }

        public override string Set(string input)
        {
            _content = new ArgumentsBracketExpression(_query);
            return _content.Set(input);
        }

        public override Value Perform(Value left)
        {
            return left;
        }

        public override Value Handle(Expression expression, Dictionary<string, Value> symbols)
        {
            var args = _content.Interpret(symbols);
            var left = expression.Interpret(symbols);

            if(left is IFunction)
                return ((IFunction)left).Perform(_query.Context, args);

            throw new FunctionNotFoundException(expression.Input);
        }

        public Value Handle(Expression expression, Value value, Dictionary<string, Value> symbols)
        {
            var isSymbol = expression is SymbolExpression;
            var symbolName = string.Empty;
            var context = _query.Context;
            var args = _content.Interpret(symbols);

            if (isSymbol)
            {
                var sym = (SymbolExpression)expression;
                symbolName = sym.SymbolName;

                if (!context.Variables.ContainsKey(sym.SymbolName))
                    context.AssignVariable(sym.SymbolName, new MatrixValue());
            }

            var left = expression.Interpret(symbols);

            if (left is ISetFunction)
            {
                var sf = (ISetFunction)left;
                sf.Perform(_query.Context, args, value);
                return left;
            }

            throw new FunctionNotFoundException(expression.Input);
        }

        public override string ToString()
        {
            return _content.ToString();
        }

		#endregion
    }
}