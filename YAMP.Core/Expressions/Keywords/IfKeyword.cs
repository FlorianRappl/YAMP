namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using YAMP.Errors;

    /// <summary>
    /// The class for the if keyword. Basic syntax:
    /// if ( CONDITION ) STATEMENT
    /// </summary>
	class IfKeyword : BodyKeyword
    {
        #region ctor

        public IfKeyword()
            : base("if")
		{
		}

        public IfKeyword(Int32 line, Int32 column, QueryContext query)
            : this()
		{
			Query = query;
            StartLine = line;
            StartColumn = column;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets the condition statement for each iteration.
        /// </summary>
        public Statement Condition { get; private set; }

        /// <summary>
        /// Gets the else block for the if statement.
        /// </summary>
        public ElseKeyword Else { get; internal set; }

        #endregion

        #region Methods

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var kw = new IfKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            var index = engine.Advance(Token.Length).Skip().Pointer;
            var chars = engine.Characters;

            if (index == chars.Length)
            {
                kw.Length = engine.Pointer - start;
                engine.AddError(new YAMPIfArgumentsMissing(engine), kw);
                return kw;
            }

            if (chars[index] == '(')
            {
                var ln = engine.CurrentLine;
                var col = engine.CurrentColumn;
                kw.Condition = engine.Advance().ParseStatement(')', e => new YAMPBracketNotClosedError(ln, col));

                if (kw.Condition.Container == null || !kw.Condition.Container.HasContent)
                {
                    engine.AddError(new YAMPIfArgumentsMissing(engine), kw);
                }

                kw.Body = engine.ParseStatement();
            }
            else
            {
                engine.AddError(new YAMPIfArgumentsMissing(engine), kw);
            }

            kw.Length = engine.Pointer - start;
            return kw;
        }

        public override Value Interpret(IDictionary<String, Value> symbols)
        {
            var condition = Condition.Interpret(symbols);

            if (condition != null && condition is ScalarValue)
            {
                var boolean = (ScalarValue)condition;

                if (boolean.IsTrue)
                {
                    return Body.Interpret(symbols);
                }
            }

            if (Else != null)
            {
                return Else.Body.Interpret(symbols);
            }

            return null;
        }

        #endregion

        #region String Representations

        public override String ToCode()
        {
            var sb = new StringBuilder();
            sb.Append(Token).Append("(");
            sb.Append(Condition.Container.ToCode());
            sb.AppendLine(")");
            sb.AppendLine(Body.ToCode());
            return sb.ToString();
        }

        #endregion
    }
}