using System;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace YAMP
{
	class AbsExpression : Expression
	{
		BracketExpression _child;
		AbsFunction abs;
		
		public AbsExpression () : base(@"\|.*\|")
		{
			abs = new AbsFunction();
		}

        public override Expression Create(Match match)
        {
            return new AbsExpression();
        }
		
		public override Value Interpret (Hashtable symbols)
		{
			var value = _child.Interpret(symbols);
			return abs.Perform(value);
		}
		
		public override string Set (string input)
		{		
			var brackets = 0;
			var sb = new StringBuilder();
			
			for(var i = 1; i < input.Length; i++)
			{
				if(input[i] == ')' || input[i] == ']' || input[i] == '}')
					brackets--;
				else if(input[i] == '(' || input[i] == '[' || input[i] == '{')
					brackets++;
				else if(brackets == 0 && input[i] == '|')
				{
					_input = sb.ToString();
					var _tree = new ParseTree(_input);
					_child = new BracketExpression(_tree); 
					return input.Substring(i + 1);
				}

				sb.Append(input[i]);
			}
			
			throw new BracketException("|", input);
		}
		
		public override string ToString ()
		{
			return "| | [ ExpressionType = Abs ]\n" + _child.ToString();
		}
	}
}