namespace YAMP
{
    using System.Collections.Generic;
    using System.Text;
    using YAMP.Errors;

    /// <summary>
    /// The while keyword has its logic defined here. The basic syntax:
    /// while ( CONDITION ) STATEMENT
    /// </summary>
    class WhileKeyword : BreakableKeyword
    {
        #region Fields

        bool __break;

        #endregion

        #region ctor

        public WhileKeyword() : base("while")
        {
        }

        public WhileKeyword(int line, int column, QueryContext query)
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
        /// Gets the status of the while-loop. Is it a do-while loop?
        /// </summary>
        public bool IsDoWhile { get; private set; }

        #endregion

        #region Methods

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var kw = new WhileKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            var index = engine.Advance(Token.Length).Skip().Pointer;
            var chars = engine.Characters;

            if (index == chars.Length)
            {
                kw.Length = engine.Pointer - start;
                engine.AddError(new YAMPWhileArgumentsMissing(engine), kw);
                return kw;
            }

            if (chars[index] == '(')
            {
                var ln = engine.CurrentLine;
                var col = engine.CurrentColumn;
                kw.Condition = engine.Advance().ParseStatement(')', e => new YAMPBracketNotClosedError(ln, col));

                if (kw.Condition.Container == null || !kw.Condition.Container.HasContent)
                {
                    engine.AddError(new YAMPWhileArgumentsMissing(engine), kw);
                }

                SetMarker(engine);
                kw.Body = engine.ParseStatement();
                UnsetMarker(engine);

                if (engine.LastStatement != null && engine.LastStatement.IsKeyword<DoKeyword>())
                {
                    if (kw.Body.IsEmpty)
                    {
                        IsDoWhile = true;
                        engine.LastStatement.GetKeyword<DoKeyword>().While = kw;
                    }
                    else
                    {
                        engine.AddError(new YAMPDoWhileNotEmptyError(engine), kw);
                    }
                }
            }
            else
            {
                engine.AddError(new YAMPWhileArgumentsMissing(engine), kw);
            }

            kw.Length = engine.Pointer - start;
            return kw;
        }

        public override void Break()
        {
            __break = true;
        }

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            if (IsDoWhile)
                return null;

            __break = false;

            while (InterpretCondition(symbols))
            {
                Body.Interpret(symbols);

                if (__break)
                    break;
            }

            return null;
        }

        public bool InterpretCondition(Dictionary<string, Value> symbols)
        {
            var condition = Condition.Interpret(symbols);

            if (condition != null && condition is ScalarValue)
                return ((ScalarValue)condition).IsTrue;

            return false;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            var sb = new StringBuilder();
            sb.Append(Token).Append("(");
            sb.Append(Condition.Container.ToCode());
            sb.AppendLine(")").AppendLine(Body.ToCode());
            return sb.ToString();
        }

        #endregion
    }
}