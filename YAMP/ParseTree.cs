using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
	public class ParseTree
	{
		#region Constants

		const string SPACING = "  ";

		#endregion

		#region Members
		
		Operator _operator;
		Expression[] _expressions;
		string _input;
		int _offset;
		bool _isList;

		#endregion

		#region Properties

		public bool IsList
		{
			get { return _isList; }
		}

		public Operator Operator
		{
			get
			{
				return _operator;
			}
			set
			{
				_operator = value;
			}
		}
		
		public Expression[] Expressions
		{
			get
			{
				return _expressions;
			}
			set
			{
				_expressions = value;
			}
		}

		#endregion

		#region ctor
		
		public ParseTree(Operator op, Expression[] expressions)
		{
			_operator = op;
			_expressions = expressions;
		}
		
		public ParseTree(Operator op, Expression expression)
		{
			_operator = op;
			_expressions = new Expression[] { expression };
		}
		
		public ParseTree(Operator op, Expression left, Expression right)
		{
			_operator = op;
			_expressions = new Expression[] { left, right };
		}

		public ParseTree (string input, int offset) : this(input, offset, false)
		{
		}

		public ParseTree (string input) : this(input, 0, false)
		{
		}

		public ParseTree (string input, bool isList) : this(input, 0, isList)
		{
		}

		public ParseTree(string input, int offset, bool isList)
		{
			_offset = offset;
			_input = input;
			_isList = isList;

			if(_input.Length > 0 && _input[0] == '-')
				_input = "0" + _input;

			Parse();
		}
		
		#endregion
		
		void Parse()
		{			
			BracketExpression bracket = null;
			var ops = new List<Operator>();
			var exps = new List<Expression>();
			var shadow = _input;
			var takeop = false;
			var maxLevel = -100;
			var maxIndex = 0;
			var expIndex = 0;
			var tmpExp = 0;
            var offset = _offset;

            if (_input.Length == 0)
            {
                _expressions = new Expression[] { new EmptyExpression() };
                return;
            }

			while(shadow.Length > 0)
			{
				if(shadow[0] == ' ')
				{
					offset++;
					shadow = shadow.Substring(1);
					continue;
				}

				if(shadow.Length > 1)
				{
					var key = new string(new char[] { shadow[0], shadow[1] });

					if(Tokens.Instance.Sanatizers.ContainsKey(key))
					{
						offset++;
						shadow = Tokens.Instance.Sanatizers[key] + shadow.Substring(2);
						continue;
					}
				}

				if(takeop)
				{
					var op = Tokens.Instance.FindOperator(shadow);

					if(op.Level >= maxLevel)
					{
						maxLevel = op.Level;
						maxIndex
							= ops.Count;
						expIndex = exps.Count;
					}

					op.IsList = _isList;
					ops.Add(op);
					shadow = op.Set(shadow);
					offset += op.Input.Length;
					takeop = !op.ExpectExpression;
				}
				else
				{
					var exp = Tokens.Instance.FindExpression(shadow);
					exp.Offset = offset;
					exps.Add(exp);
					shadow = exp.Set(shadow);
					offset += exp.Input.Length;
					takeop = true;
				}
            }

            expIndex--;

			while(ops.Count > 1)
            {
				if(ops[maxIndex].ExpectExpression)
				{
					tmpExp = expIndex + 1;
					bracket = new BracketExpression(new ParseTree(ops[maxIndex], exps[expIndex], exps[tmpExp]));
					exps.RemoveAt(tmpExp);
				}
				else
				{
					bracket = new BracketExpression(new ParseTree(ops[maxIndex], exps[expIndex]));
				}

				exps.RemoveAt(expIndex);
				exps.Insert(expIndex, bracket);
				ops.RemoveAt(maxIndex);
				tmpExp = 1;
				expIndex = 0;
				maxIndex = 0;
				maxLevel = ops[0].Level;

				for(var i = 1; i < ops.Count; i++)
				{	
					if(maxLevel <= ops[i].Level)
					{
						expIndex = tmpExp;
						maxIndex = i;
						maxLevel = ops[i].Level;
                    }

                    if (ops[i].ExpectExpression)
                        tmpExp++;
				}
			}

			_expressions = exps.ToArray();
			
			if(ops.Count == 1)
				_operator = ops[0];
		}
		
		public override string ToString ()
		{
			var sb = new StringBuilder();
			sb.Append(PrintExpression(Expressions[0]));
			
			if(Operator != null)
			{
				sb.Append(SPACING).AppendLine(Operator.ToString());
				
				if(Expressions.Length == 2)
					sb.Append(PrintExpression(Expressions[1]));
			}
			
			return sb.ToString();
		}
		
		string PrintExpression(Expression exp)
		{
			var lines = exp.ToString().Split(new string [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var sb = new StringBuilder();
			
			foreach(var line in lines)
				sb.Append(SPACING).AppendLine(line);
			
			return sb.ToString();
		}
	}
}

