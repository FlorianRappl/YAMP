using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
	public class ParseTree
	{
		const string SPACING = "  ";
		
		Operator[] operators;
		AbstractExpression[] expressions;
		string _input;

		public Operator[] Operators
		{
			get
			{
				return this.operators;
			}
			set
			{
				operators = value;
			}
		}
		
		public AbstractExpression[] Expressions
		{
			get
			{
				return this.expressions;
			}
			set
			{
				expressions = value;
			}
		}
		
		public ParseTree(Operator[] operators, AbstractExpression[] expressions)
		{
			this.operators = operators;
			this.expressions = expressions;
		}

		public ParseTree (string input)
		{
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

					ops.Add(op);
					shadow = op.Set(shadow);
				}
				else
				{
					var exp = Tokens.Instance.FindExpression(shadow);
					exps.Add(exp);
					shadow = exp.Set(shadow);
				}
				
				takeop = !takeop;
			}
			
			if(exps.Count != ops.Count + 1)
				throw new ParseException(_input.Length - 1, _input.Substring(Math.Max(_input.Length - 3, 0)));

			while(ops.Count > 1)
			{
				var bracket = new BracketExpression(new ParseTree(
					new Operator[] { ops[maxIndex] },
					new AbstractExpression[] { exps[maxIndex], exps[maxIndex + 1] }
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

			operators = ops.ToArray();
			expressions = exps.ToArray();
		}
		
		public override string ToString ()
		{
			var sb = new StringBuilder();
			sb.Append(PrintExpression(Expressions[0]));
			
			for(var i = 0; i < Operators.Length; i++)
			{
				sb.Append(SPACING).AppendLine(Operators[i].ToString());
				sb.Append(PrintExpression(Expressions[i + 1]));
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

