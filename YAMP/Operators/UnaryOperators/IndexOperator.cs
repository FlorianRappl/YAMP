using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    class IndexOperator : UnaryOperator
    {
		BracketExpression _bracket;
		object[][] _indices;
		Type[] _types;
		int _dimX;
		int _dimY;
        ParseContext _context;

		static readonly Type mt = typeof(MatrixValue);
        static readonly Type at = typeof(ArgumentsValue);

		public override string Input
		{
			get 
			{
				return _bracket.Input;
			}
		}
		
        public IndexOperator() : base("[", 1000)
		{
            _context = ParseContext.Default;
		}

        public IndexOperator(ParseContext context) : base("[", 1000)
        {
            _context = context;
        }

        public override Operator Create()
        {
            return new IndexOperator();
        }

        public override Operator Create(ParseContext context)
        {
            return new IndexOperator(context);
        }
		
		public override string Set (string input)
		{
            _bracket = new BracketExpression(_context);
			return _bracket.Set(input, true);
		}
		
		public override Value Perform (Value left)
		{
			if(left is MatrixValue)
			{
                if (_indices.Length == 1)
                    return mt.GetProperty("Item", _types).GetValue(left, _indices[0]) as ScalarValue;

				var m = new MatrixValue(_dimY, _dimX);

				for(var k = 0; k < _indices.Length; k++)
					m[k + 1] = mt.GetProperty("Item", _types).GetValue(left, _indices[k]) as ScalarValue;

				return m;
			}
            else if(left is ArgumentsValue)
            {
                if (_indices.Length != 1)
                    throw new ArgumentsException("[]", _indices.Length);

                return at.GetProperty("Item", _types).GetValue(left, _indices[0]) as Value;
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
					throw new AssignmentException(Op, "Cannot assign non-numeric values to numeric matrices");

				if(frm.DimensionX != _dimX)
					throw new DimensionException(frm.DimensionX, _dimX);

				if(frm.DimensionY != _dimY)
					throw new DimensionException(frm.DimensionY, _dimY);

				for(var k = 0; k < _indices.Length; k++)
					Assign(left, frm[k + 1], _indices[k]);

				return left;
			}
            /*
            else if (left is ArgumentsValue)
            {
                at.GetProperty("Item", _types).SetValue(left, value, _indices[0]);
            }*/
			
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
            else if (value is ArgumentsValue)
                return (value as ArgumentsValue).Length;

			throw new OperationNotSupportedException("[]", value);
		}
		
		int GetDimX (Value value)
		{
            if (value is StringValue)
                return (value as StringValue).Length;
            else if (value is MatrixValue)
                return (value as MatrixValue).DimensionX;
            else if (value is ArgumentsValue)
                return (value as ArgumentsValue).Length;
			
			throw new OperationNotSupportedException("[]", value);
		}
		
		int GetDimY (Value value)
		{
			if(value is StringValue)
				return 1;
			else if(value is MatrixValue)
                return (value as MatrixValue).DimensionY;
            else if (value is ArgumentsValue)
                return 1;
			
			throw new OperationNotSupportedException("[]", value);
		}
		
		void GetIndex(Hashtable symbols, Value left)
		{
			Value _value;

			if (_bracket.Tree.Operator != null)
				_value = _bracket.Tree.Operator.Evaluate(_bracket.Tree.Expressions, symbols);
			else
				_value = _bracket.Tree.Expressions[0].Interpret(symbols);
			
			if(_value is ScalarValue || _value is MatrixValue)
			{
				if(_value is MatrixValue)
				{
					var m = _value as MatrixValue;

					if(m.DimensionX == GetDimX(left) && m.DimensionY == GetDimY(left))
					{
						LogicalSubscripting(m);
						return;
					}
				}

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

		void LogicalSubscripting(MatrixValue m)
		{
			_types = new Type[] { typeof(int), typeof(int) };
			var idx = new List<object[]>();

			for(var i = 1; i <= m.DimensionX; i++)
				for(var j = 1; j <= m.DimensionY; j++)
					if(m[j, i].Value != 0.0)
						idx.Add(new object[] { j, i });

			_dimX = 1;
			_dimY = idx.Count;
			_indices = idx.ToArray();
		}
		
		public override Value Handle (Expression expression, Hashtable symbols)
		{
			var left = expression.Interpret(symbols);
            GetIndex(symbols, left);			
			return Perform(left);
		}
		
		public Value Handle (Expression expression, Value value, Hashtable symbols)
		{
			if(expression is SymbolExpression)
			{
				var sym = expression as SymbolExpression;

                if (sym.IsSymbol && !_context.Variables.ContainsKey(sym.SymbolName))
                    _context.AssignVariable(sym.SymbolName, new MatrixValue(1, 1));
			}

			var left = expression.Interpret(symbols);
			GetIndex(symbols, left);
			return Perform(left, value);
		}

		public override string ToString ()
		{
			return base.ToString() + Environment.NewLine + _bracket.ToString();
		}
    }
}