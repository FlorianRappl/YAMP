using System;
using System.Text;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// Presents the class for string expressions "...".
    /// </summary>
	class StringExpression : Expression
    {
        #region Members

        bool literal;
        string value;

        #endregion

        #region ctor

        public StringExpression()
		{
		}

        public StringExpression(string content)
        {
            value = content;
        }

        public StringExpression(ParseEngine engine) : base(engine)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value if the string literal (@) was used.
        /// </summary>
        public bool IsLiteral
        {
            get { return literal; }
        }

        #endregion

        #region Methods

		public override Value Interpret(Dictionary<string, Value> symbols)
		{
            return new StringValue(value);
		}

        public override Expression Scan(ParseEngine engine)
        {
            var chars = engine.Characters;
            var start = engine.Pointer;

            if (chars[start] == '"' || (chars[start] == '@' && start + 1 < chars.Length && chars[start + 1] == '"'))
            {
                var index = start;
                var exp = new StringExpression(engine);
                var escape = false;
                var terminated = false;
                var sb = new StringBuilder();

                if (chars[index] == '@')
                {
                    index += 2;
                    exp.literal = true;
                }
                else
                    index++;

                while (index < chars.Length)
                {
                    if (!literal && !escape && chars[index] == '\\')
                        escape = true;
                    else if (!escape && chars[index] == '"')
                    {
                        terminated = true;
                        index++;
                        break;
                    }
                    else if (escape)
                    {
                        switch (chars[index])
                        {
                            case 't':
                                sb.Append("\t");
                                break;
                            case 'n':
                                sb.AppendLine();
                                break;
                            case '\\':
                            case '"':
                                sb.Append(chars[index]);
                                break;
                            default:
                                engine.SetPointer(index);
                                engine.AddError(new YAMPEscapeSequenceNotFoundError(engine, chars[index]), exp);
                                break;
                        }

                        escape = false;
                    }
                    else
                        sb.Append(chars[index]);

                    index++;
                }

                if (!terminated)
                    engine.AddError(new YAMPStringNotTerminatedError(engine), exp);

                exp.value = sb.ToString();
                exp.Length = index - start;
                engine.SetPointer(index);
                return exp;
            }

            return null;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            if(IsLiteral)
                return "@\"" + value + '"';

            return '"' + 
                     value.Replace("\t", "\\t")
                          .Replace("\n", "\\n")
                          .Replace("\\", "\\\\")
                          .Replace("\"", "\\\"")
                    + '"';
        }

        #endregion
    }
}

