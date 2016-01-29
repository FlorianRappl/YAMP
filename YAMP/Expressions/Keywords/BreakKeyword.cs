using System;
using System.Collections.Generic;

namespace YAMP
{
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

        public BreakKeyword(int line, int column, QueryContext query)
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
                engine.AddError(new YAMPKeywordNotPossible(engine, Token), kw);

            return kw;
        }

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            var ctx = GetBreakableContext(Query);
            ctx.CurrentStatement.GetKeyword<BreakableKeyword>().Break();
            return null;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            return Token;
        }

        #endregion

        #region Helpers

        bool IsBreakable(ParseEngine engine)
        {
            if (engine.HasMarker(Marker.Breakable))
                return true;

            if (engine.Parent != null)
                return IsBreakable(engine.Parent);

            return false;
        }

        QueryContext GetBreakableContext(QueryContext context)
        {
            if (context.CurrentStatement.IsKeyword<BreakableKeyword>())
                return context;

            context.Stop();
            return GetBreakableContext(context.Parent);
        }

        #endregion
    }
}
