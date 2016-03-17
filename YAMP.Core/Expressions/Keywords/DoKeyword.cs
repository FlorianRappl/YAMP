namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The class representing the do keyword for do {...} while(...); loops. Basic syntax:
    /// do STATEMENT while ( CONDITION) ;
    /// </summary>
    class DoKeyword : BreakableKeyword
    {
        #region Fields

        bool __break;

        #endregion

        #region ctor

        public DoKeyword()
            : base("do")
        {
        }

        public DoKeyword(Int32 line, Int32 column, QueryContext query)
            : this()
        {
            StartLine = line;
            StartColumn = column;
            Query = query;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the associated condition in form of the while keyword.
        /// </summary>
        public WhileKeyword While { get; internal set; }

        #endregion

        #region Methods

        public override Value Interpret(IDictionary<String, Value> symbols)
        {
            __break = false;

            do
            {
                Body.Interpret(symbols);
            }
            while (!__break && While.InterpretCondition(symbols));

            return null;
        }

        public override void Break()
        {
            __break = true;
        }

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var kw = new DoKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            var index = engine.Advance(Token.Length).Pointer;
            kw.Body = engine.ParseStatement();
            kw.Length = engine.Pointer - start;
            return kw;
        }

        #endregion

        #region String Representations

        public override String ToCode()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Token);
            sb.AppendLine(Body.ToCode());
            return sb.ToString();
        }

        #endregion
    }
}
