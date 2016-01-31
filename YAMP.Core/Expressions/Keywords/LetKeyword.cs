namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Errors;

    /// <summary>
    /// The let keyword to create local variables. Basic syntax:
    /// let NAME [OP STATEMENT];
    /// </summary>
    class LetKeyword : Keyword
    {
        #region Fields

        SymbolExpression name;

        #endregion

        #region ctor

        public LetKeyword()
            : base("let")
        {
        }

        public LetKeyword(int line, int column, QueryContext query)
            : this()
        {
            Query = query;
            StartLine = line;
            StartColumn = column;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the created local variable.
        /// </summary>
        public string Name
        {
            get { return name.SymbolName; }
        }

        #endregion

        #region Methods

        public override Value Interpret(Dictionary<String, Value> symbols)
        {
            if (symbols.ContainsKey(Name))
            {
                symbols.Remove(Name);
            }

            symbols.Add(Name, ScalarValue.Empty);
            return null;
        }

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var kw = new LetKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            engine.Advance(Token.Length);
            kw.Length = engine.Pointer - start;
            engine.Skip();
            start = engine.Pointer;
            kw.name = engine.Elements.FindExpression<SymbolExpression>().Scan(engine) as SymbolExpression;

            if (kw.name == null)
            {
                engine.AddError(new YAMPVariableNameMissing(engine), kw);
                return kw;
            }

            engine.SetPointer(start);
            return kw;
        }

        #endregion

        #region String Representation

        public override string ToCode()
        {
            return "let ";
        }

        #endregion
    }
}
