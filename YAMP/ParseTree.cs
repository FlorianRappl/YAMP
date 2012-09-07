using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
	public class ParseTree
	{
		const string SPACING = "  ";
		
		Operator _operator;
		AbstractExpression[] _expressions;
		string _input;
		int _offset;

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
		
		public AbstractExpression[] Expressions
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
		
		public ParseTree(Operator op, AbstractExpression[] expressions)
		{
			_operator = op;
			_expressions = expressions;
		}
		
		public ParseTree(Operator op, AbstractExpression expression)
		{
			_operator = op;
			_expressions = new AbstractExpression[] { expression };
		}
		
		public ParseTree(Operator op, AbstractExpression left, AbstractExpression right)
		{
			_operator = op;
			_expressions = new AbstractExpression[] { left, right };
		}

		public ParseTree (string input, int offset)
		{
			_offset = offset;
			_input = input;
			Parse();
		}

		public ParseTree (string input)
		{
			_offset = 0;
			_input = input;
			Parse();
		}
		
		void Parse()
		{			
			var ops = new List<Operator>();
			var exps = new List<AbstractExpression>();
			var shadow = _input;
			var takeop = false;
			var maxLevel = 0;
			var maxIndex = 0;
			var offset = _offset;
			
			if(string.IsNullOrEmpty(_input))
				exps.Add(new EmptyExpression());

			while(shadow.Length > 0)
			{
				if(takeop)
				{
					var op = Tokens.Instance.FindOperator(shadow);

					if(op.Level >= maxLevel)
					{
						maxLevel = op.Level;
						maxIndex = ops.Count;
					}

					shadow = op.Set(shadow);
					offset += op.Op.Length;
					
					if(op is UnaryOperator)
					{
						if(exps.Count == 0)
							throw new ParseException(offset, shadow.Substring(0, Math.Min(shadow.Length, 3)));
						
						var tree = new ParseTree(op, exps[exps.Count - 1]);
						var bracket = new BracketExpression(tree);
						exps[exps.Count - 1] = bracket;		
						continue;
					}
					else
						ops.Add(op);
				}
				else
				{
					var exp = Tokens.Instance.FindExpression(shadow);
					exp.Offset = offset;
					var old = shadow.Length;
					exps.Add(exp);
					shadow = exp.Set(shadow);
					offset += (old - shadow.Length);
				}
				
				takeop = !takeop;
			}
			
			if(exps.Count != ops.Count + 1)
				throw new ParseException(offset, _input.Substring(Math.Max(_input.Length - 3, 0)));

			while(ops.Count > 1)
			{
				var bracket = new BracketExpression(new ParseTree(
					ops[maxIndex],
					exps[maxIndex], exps[maxIndex + 1]
				));
				exps.RemoveAt(maxIndex + 1);
				exps.RemoveAt(maxIndex);
				exps.Insert(maxIndex, bracket);
				ops.RemoveAt(maxIndex);
				maxIndex = 0;
				maxLevel = ops[0].Level;

				for(var i = 1; i < ops.Count; i++)
				{			
					if(maxLevel <= ops[i].Level)
					{
						maxIndex = i;
						maxLevel = ops[i].Level;
					}
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
		
		string PrintExpression(AbstractExpression exp)
		{
			var lines = exp.ToString().Split(new string [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var sb = new StringBuilder();
			
			foreach(var line in lines)
				sb.Append(SPACING).AppendLine(line);
			
			return sb.ToString();
		}
	}
}

