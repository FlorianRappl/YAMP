namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Errors;

    /// <summary>
    /// This is the class responsible for the break keyword. Basic syntax:
    /// break;
    /// </summary>
    class BreakKeyword : Keyword
    {
        #region ctor

        public BreakKeyword()
            : base("break")
        {
        }

        public BreakKeyword(Int32 line, Int32 column, QueryContext query)
            : this()
        {
            Query = query;
            StartLine = line;
            StartColumn = column;
            Length = Token.Length;
        }

        #endregion

        #region Methods

        public override Expression Scan(ParseEngine engine)
        {
            var kw = new BreakKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            engine.Advance(Token.Length);

            if (!IsBreakable(engine))
            {
                engine.AddError(new YAMPKeywordNotPossible(engine, Token), kw);
            }

            return kw;
        }

        public override Value Interpret(IDictionary<String, Value> symbols)
        {
            var ctx = GetBreakableContext(Query);
            ctx.CurrentStatement.GetKeyword<BreakableKeyword>().Break();
            return null;
        }

        #endregion

        #region String Representations

        public override String ToCode()
        {
            return Token;
        }

        #endregion

        #region Helpers

        Boolean IsBreakable(ParseEngine engine)
        {
            if (!engine.HasMarker(Marker.Breakable))
            {
                if (engine.Parent != null)
                {
                    return IsBreakable(engine.Parent);
                }

                return false;
            }

            return true;
        }

        QueryContext GetBreakableContext(QueryContext context)
        {
            if (!context.CurrentStatement.IsKeyword<BreakableKeyword>())
            {
                context.Stop();
                return GetBreakableContext(context.Parent);
            }

            return context;
        }

        #endregion
    }
}
