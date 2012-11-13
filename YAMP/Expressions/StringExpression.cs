using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace YAMP
{
	class StringExpression : Expression
	{
		public StringExpression () : base(@"\""")
		{
		}

        public override Expression Create(QueryContext query, Match match)
        {
            return new StringExpression();
        }
		
		public override Value Interpret (Dictionary<string, object> symbols)
		{
			return new StringValue(_input);
		}
		
		public override string Set (string input)
		{
			var escape = false;
			var sb = new StringBuilder();

			for(var i = 1; i < input.Length; i++)
			{
				if(!escape && input[i] == '\\')
					escape = true;
				else if(!escape && input[i] == '"')
				{
					_input = sb.ToString();
					return input.Length > i + 1 ? input.Substring(i + 1) : string.Empty;
				}
				else
				{
					sb.Append(input[i]);
					escape = false;
				}
			}
			
			throw new BracketException(Offset, "\"", input);
		}
	}
}

