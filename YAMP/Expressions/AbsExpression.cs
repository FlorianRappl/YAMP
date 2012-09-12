using System;
using System.Collections;

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

        public override Expression Create()
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
			
			for(var i = 1; i < input.Length; i++)
			{
				if(input[i] == ')')
					brackets--;
				else if(input[i] == '(')
					brackets++;
				
				if(brackets == 0 && input[i] == '|')
				{
					_input = input.Substring(1, i - 1);
					var _tree = new ParseTree(_input);
					_child = new BracketExpression(_tree); 
					return input.Length > i + 1 ? input.Substring(i + 1) : string.Empty;
				}
			}
			
			throw new BracketException("|", input);
		}
		
		public override string ToString ()
		{
			return "| | [ ExpressionType = Abs ]\n" + _child.ToString();
		}
	}
}