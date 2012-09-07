using System;
using System.Collections;

namespace YAMP
{
    class IndexOperator : UnaryOperator
    {
		ParseTree _tree;
		object[] _indices;
		string _input;
		
        public IndexOperator() : base("[")
		{
		}
		
		public override string Set (string input)
		{
			var brackets = 1;
			
			for(var i = 1; i < input.Length; i++)
			{
				if(input[i] == ']')
					brackets--;
				else if(input[i] == '[')
					brackets++;
				
				if(brackets == 0)
				{
					_input = input.Substring(1, i - 1);
					_tree = new ParseTree(_input);
					i++;
					return input.Length > i ? input.Substring(i) : string.Empty;
				}
			}
			
			throw new BracketException("[", input);
		}
		
		public override Value Perform (Value left)
		{
			if(left is MatrixValue)
			{
				var types = new Type[_indices.Length];
				
				for(var i = 0; i < types.Length; i++)
					types[i] = typeof(int);
				
				return left.GetType().GetProperty("Item", types).GetValue(left, _indices) as Value;
			}
			
			throw new OperationNotSupportedException("[]", left);
		}
		
		public Value Perform (Value left, Value value)
		{
			if(left is MatrixValue)
			{
				var types = new Type[_indices.Length];
				
				for(var i = 0; i < types.Length; i++)
					types[i] = typeof(int);
				
				left.GetType().GetProperty("Item", types).SetValue(left, value, _indices);
				return left;
			}
			
			throw new OperationNotSupportedException("[]", left);
		}
		
		void GetIndex(Hashtable symbols)
		{
			Value _value;
			
            if (_tree.Operator != null)
                _value = _tree.Operator.Evaluate(_tree.Expressions, symbols);
            else
				_value = _tree.Expressions[0].Interpret(symbols);
			
			if(_value is ScalarValue)
			{
				var index = (int)(_value as ScalarValue).Value;
				_indices = new object[] { index };
			}
			else if(_value is MatrixValue)
			{
				var i = (int)(_value as MatrixValue)[1].Value;
				var j = (int)(_value as MatrixValue)[2].Value;
				_indices = new object[] { i, j };
			}
			else
				throw new OperationNotSupportedException("[]", _value);
		}
		
		public override Value Handle (AbstractExpression expression, Hashtable symbols)
		{
            GetIndex(symbols);			
			return Perform(expression.Interpret(symbols));
		}
		
		public Value Handle (AbstractExpression expression, Value value, Hashtable symbols)
		{
            GetIndex(symbols);
			
			if(expression is SymbolExpression)
			{
				var sym = expression as SymbolExpression;
				
				if(sym.IsSymbol && !Tokens.Instance.Variables.ContainsKey(sym.SymbolName))
					Tokens.Instance.AssignVariable(sym.SymbolName, new MatrixValue(1, 1));
			}
			
			return Perform(expression.Interpret(symbols), value);
		}
    }
}