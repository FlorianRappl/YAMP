using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace YAMP
{
	class StringExpression : Expression
    {
        #region Members

        bool literal;

        #endregion

        #region ctor

        public StringExpression() : base(@"@?\"".*""")
		{
		}

        #endregion

        #region Properties

        public bool Literal
        {
            get { return literal; }
        }

        #endregion

        #region Methods

        public override Expression Create(QueryContext query, Match match)
		{
			return new StringExpression();
		}

		public override Value Interpret(Dictionary<string, Value> symbols)
		{
			return new StringValue(_input);
		}
		
		public override string Set(string input)
		{
			var escape = false;
			var sb = new StringBuilder();
            var offset = 1;

            if (input[0] == '@')
            {
                offset++;
                literal = true;
            }

			for(var i = offset; i < input.Length; i++)
			{
				if(!literal && !escape && input[i] == '\\')
					escape = true;
				else if(!escape && input[i] == '"')
				{
					_input = sb.ToString();
					return input.Length > i + 1 ? input.Substring(i + 1) : string.Empty;
				}
				else if (escape)
				{
					switch (input[i])
					{
						case 't':
							sb.Append("\t");
							break;
						case 'n':
							sb.AppendLine();
							break;
						case '\\':
						case '"':
							sb.Append(input[i]);
							break;
						default:
							throw new EscapeSequenceNotDetectedException(input[i]);
					}

					escape = false;
				}
				else
				{
					sb.Append(input[i]);
				}
			}
			
			throw new BracketException(Offset, "\"", input);
        }

        #endregion
    }
}

