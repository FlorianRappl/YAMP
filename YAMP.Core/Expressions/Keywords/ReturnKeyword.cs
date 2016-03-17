namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the return keyword to cancel the current execution and return
    /// the given expression [if any].
    /// </summary>
    class ReturnKeyword : BodyKeyword
    {
        #region ctor

        public ReturnKeyword()
            : base("return")
        {
        }

        public ReturnKeyword(Int32 line, Int32 column, QueryContext query)
            : this()
        {
            Query = query;
            StartLine = line;
            StartColumn = column;
        }

        #endregion

        #region Methods

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var kw = new ReturnKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            engine.Advance(Token.Length);
            kw.Body = engine.ParseStatement();
            kw.Body.IsMuted = false;
            kw.Length = engine.Pointer - start;
            return kw;
        }

        public override Value Interpret(IDictionary<String, Value> symbols)
        {
            StopAllContexts(Query);
            return Body.Interpret(symbols);
        }

        #endregion

        #region String Representations

        public override String ToCode()
        {
            return Token + " " + Body.ToCode();
        }

        #endregion

        #region Helpers

        void StopAllContexts(QueryContext context)
        {
            context.Stop();
            context.CurrentStatement.IsMuted = false;

            if (context.CurrentStatement.IsKeyword<BreakableKeyword>())
            {
                context.CurrentStatement.GetKeyword<BreakableKeyword>().Break();
            }

            if (context.Parent != null)
            {
                StopAllContexts(context.Parent);
            }
        }

        #endregion
    }
}
