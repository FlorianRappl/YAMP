namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using YAMP.Errors;

    /// <summary>
    /// The for keyword with the corresponding loop construct. Basic syntax:
    /// for ( INIT ; CONDITION ; END ) STATEMENT
    /// </summary>
    class ForKeyword : BreakableKeyword
    {
        #region Fields

        Boolean __break;

        #endregion

        #region ctor

        public ForKeyword()
            : base("for")
        {
        }

		public ForKeyword(Int32 line, Int32 column, QueryContext query)
            : this()
		{
			Query = query;
            StartLine = line;
            StartColumn = column;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets the initialization statement before the iterations.
        /// </summary>
        public Statement Initialization { get; private set; }

        /// <summary>
        /// Gets the condition statement for each iteration.
        /// </summary>
        public Statement Condition { get; private set; }

        /// <summary>
        /// Gets the ending statement of each iteration.
        /// </summary>
        public Statement End { get; private set; }

        #endregion

        #region Methods

        public override Expression Scan(ParseEngine engine)
        {
            var kw = new ForKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            var start = engine.Pointer;
            var index = engine.Advance(Token.Length).Skip().Pointer;
            var chars = engine.Characters;

            if (index == chars.Length)
            {
                kw.Length = engine.Pointer - start;
                engine.AddError(new YAMPForArgumentsMissing(engine));
                return kw;
            }
            else if (chars[index] == '(')
            {
                var ln = engine.CurrentLine;
                var col = engine.CurrentColumn;
                kw.Initialization = engine.Advance().ParseStatement();
                kw.Condition = engine.ParseStatement();
                kw.Condition.IsMuted = false;
                kw.End = engine.ParseStatement(')', e => new YAMPBracketNotClosedError(ln, col));
                SetMarker(engine);
                kw.Body = engine.ParseStatement();
                UnsetMarker(engine);
            }
            else
            {
                engine.AddError(new YAMPForArgumentsMissing(engine));
            }

            kw.Length = engine.Pointer - start;
            return kw;
        }

        public override void Break()
        {
            __break = true;
        }

        public override Value Interpret(IDictionary<String, Value> symbols)
        {
            Initialization.Interpret(symbols);
            __break = false;

            while (InterpretCondition(symbols))
            {
                Body.Interpret(symbols);

                if (__break)
                {
                    break;
                }

                End.Interpret(symbols);
            }

            return null;
        }

        public Boolean InterpretCondition(IDictionary<String, Value> symbols)
        {
            var condition = Condition.Interpret(symbols);

            if (condition != null && condition is ScalarValue)
            {
                return ((ScalarValue)condition).IsTrue;
            }

            return false;
        }

        #endregion

        #region String Representations

        public override String ToCode()
        {
            var sb = new StringBuilder();
            sb.Append(Token).Append("(");
            sb.Append(Initialization.ToCode()).Append(Condition.ToCode()).Append(End.Container.ToCode());
            sb.AppendLine(")");
            sb.AppendLine(Body.ToCode());
            return sb.ToString();
        }

        #endregion
    }
}