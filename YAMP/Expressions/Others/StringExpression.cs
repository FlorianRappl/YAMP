using System;
using System.Text;
using System.Collections;

namespace YAMP
{
	public class StringExpression : AbstractExpression
	{
		public StringExpression () : base(@"\""")
		{
		}
		
		public override Value Interpret (Hashtable symbols)
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

