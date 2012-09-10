using System;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    class IndexOperator : UnaryOperator
    {
		ParseTree _tree;
		object[][] _indices;
		Type[] _types;
		string _input;
		int _dimX;
		int _dimY;

		static readonly Type mt = typeof(MatrixValue);

		public override string Input
		{
			get 
			{
				return "[" + _input + "]";
			}
		}
		
        public IndexOperator() : base("[", 0)
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
					_tree = new ParseTree(_input, true);
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
				var m = new MatrixValue(_dimY, _dimX);

				for(var k = 0; k < _indices.Length; k++)
					m[k + 1] = mt.GetProperty("Item", _types).GetValue(left, _indices[k]) as ScalarValue;

				return m;
			}
			
			throw new OperationNotSupportedException("[]", left);
		}
		
		public Value Perform (Value left, Value value)
		{
			if(left is MatrixValue)
			{
				var frm = new MatrixValue(1,1);

				if (value is ScalarValue)
					frm[1] = value as ScalarValue;
				else if (value is MatrixValue)
					frm = value as MatrixValue;
				else
					throw new AssignmentException("Cannot assign non-numeric values to numeric matrices");

				if(frm.DimensionX != _dimX)
					throw new DimensionException(frm.DimensionX, _dimX);

				if(frm.DimensionY != _dimY)
					throw new DimensionException(frm.DimensionY, _dimY);

				for(var k = 0; k < _indices.Length; k++)
					Assign(left, frm[k + 1], _indices[k]);

				return left;
			}
			
			throw new OperationNotSupportedException("[]", left);
		}

		void Assign(Value to, Value value, object[] index)
		{
			mt.GetProperty("Item", _types).SetValue(to, value, index);
		}

		int GetLength (Value value)
		{
			if(value is StringValue)
				return (value as StringValue).Length;
			else if(value is MatrixValue)
				return (value as MatrixValue).Length;

			throw new OperationNotSupportedException("[]", value);
		}
		
		int GetDimX (Value value)
		{
			if(value is StringValue)
				return (value as StringValue).Length;
			else if(value is MatrixValue)
				return (value as MatrixValue).DimensionX;
			
			throw new OperationNotSupportedException("[]", value);
		}
		
		int GetDimY (Value value)
		{
			if(value is StringValue)
				return 1;
			else if(value is MatrixValue)
				return (value as MatrixValue).DimensionY;
			
			throw new OperationNotSupportedException("[]", value);
		}
		
		void GetIndex(Hashtable symbols, Value left)
		{
			Value _value;
			
            if (_tree.Operator != null)
                _value = _tree.Operator.Evaluate(_tree.Expressions, symbols);
            else
				_value = _tree.Expressions[0].Interpret(symbols);
			
			if(_value is ScalarValue || _value is MatrixValue)
			{
				_types = new Type[] { typeof(int) };
				var values = GetIndices(_value, GetLength(left));
				BuildIndices(values);
			}
			else if(_value is ArgumentsValue)
			{
				var list = _value as ArgumentsValue;

				if(list.Length != 2)
					throw new ArgumentsException("[]", list.Length);
				
				_types = new Type[] { typeof(int), typeof(int) };
				var rows = GetIndices(list.Values[0], GetDimX(left));
				var cols = GetIndices(list.Values[1], GetDimY(left));
				BuildIndices(rows, cols);
			}
			else
				throw new OperationNotSupportedException("[]", _value);
		}

		int[] GetIndices(Value indices, int maxLength)
		{
			var z = new List<int>();

			if(indices is ScalarValue)
				z.Add((indices as ScalarValue).IntValue);
			else if(indices is RangeValue)
			{
				var r = indices as RangeValue;
				var step = (int)r.Step;

				if(!r.All)
					maxLength = (int)r.End;

				for(var j = (int)r.Start; j <= maxLength; j += step)
					z.Add(j);
			}
			else if(indices is MatrixValue)
			{
				var m = indices as MatrixValue;

				for(var j = 1; j <= m.Length; j++)
					z.Add(m[j].IntValue);
			}

			return z.ToArray();
		}

		void BuildIndices (int[] idx)
		{
			_dimX = 1;
			_dimY = idx.Length;
			_indices = new object[idx.Length][];

			for(var i = 0; i < idx.Length; i++)
				_indices[i] = new object[] { idx[i] };
		}
		
		void BuildIndices (int[] rows, int[] cols)
		{
			_dimY = rows.Length;
			_dimX = cols.Length;
			_indices = new object[_dimX * _dimY][];
			var k = 0;

			for(var i = 0; i < _dimX; i++)
				for(var j = 0; j < _dimY; j++)
					_indices[k++] = new object[] { rows[j], cols[i] };
		}
		
		public override Value Handle (AbstractExpression expression, Hashtable symbols)
		{
			var left = expression.Interpret(symbols);
            GetIndex(symbols, left);			
			return Perform(left);
		}
		
		public Value Handle (AbstractExpression expression, Value value, Hashtable symbols)
		{
			if(expression is SymbolExpression)
			{
				var sym = expression as SymbolExpression;
				
				if(sym.IsSymbol && !Tokens.Instance.Variables.ContainsKey(sym.SymbolName))
					Tokens.Instance.AssignVariable(sym.SymbolName, new MatrixValue(1, 1));
			}

			var left = expression.Interpret(symbols);
			GetIndex(symbols, left);
			return Perform(left, value);
		}

		public override string ToString ()
		{
			return base.ToString() + Environment.NewLine + _tree.ToString();
		}
    }
}