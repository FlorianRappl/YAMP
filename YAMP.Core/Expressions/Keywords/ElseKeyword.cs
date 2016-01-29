using System;
using System.Collections.Generic;
using System.Text;

namespace YAMP
{
    /// <summary>
    /// The else keyword - can (and should) only be instantiated by the if keyword.
    /// Basic syntax: else STATEMENT
    /// Can only be used after an IF or ELSE IF
    /// </summary>
    class ElseKeyword : BodyKeyword
    {
        #region ctor

        public ElseKeyword()
            : base("else")
        {

        }

        public ElseKeyword(int line, int column, QueryContext query)
            : this()
		{
			Query = query;
            StartLine = line;
            StartColumn = column;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets a boolean indicating if the body is another if-statement.
        /// </summary>
        public bool IsElseIf
        {
            get { return Body.IsKeyword<IfKeyword>(); }
        }

        /// <summary>
        /// Gets the if-keyword contained in the body.
        /// </summary>
        public IfKeyword ElseIf
        {
            get { return Body.GetKeyword<IfKeyword>(); }
        }

        #endregion

        #region Methods

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var kw = new ElseKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            kw.Body = engine.Advance(Token.Length).ParseStatement();

            if (engine.LastStatement == null)
                engine.AddError(new YAMPIfRequiredError(engine), kw);
            else if (engine.LastStatement.IsKeyword<IfKeyword>())
                engine.LastStatement.GetKeyword<IfKeyword>().Else = kw;
            else if(engine.LastStatement.IsKeyword<ElseKeyword>())
            {
                var otherwise = engine.LastStatement.GetKeyword<ElseKeyword>();

                if (otherwise.IsElseIf)
                    otherwise.ElseIf.Else = kw;
                else
                    engine.AddError(new YAMPSingleElseError(engine), kw);
            }
            else
                engine.AddError(new YAMPIfRequiredError(engine), kw);

            kw.Length = engine.Pointer - start;
            return kw;
        }

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return null;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Token).Append(Body.ToCode());
            return sb.ToString();
        }

        #endregion
    }
}
