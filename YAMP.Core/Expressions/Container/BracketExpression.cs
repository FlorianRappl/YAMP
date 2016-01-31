namespace YAMP
{
    using System.Collections.Generic;
    using YAMP.Errors;

    /// <summary>
    /// The bracket expression (...).
    /// </summary>
    class BracketExpression : TreeExpression
    {
        #region ctor

        public BracketExpression()
		{
		}

		public BracketExpression(int line, int column, int length, QueryContext query, ContainerExpression child) 
            : base(child, query, line, column)
		{
            Length = length;
		}

        #endregion

        #region Methods

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return base.Interpret(symbols) ?? new ArgumentsValue();
        }

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var chars = engine.Characters;

            if (chars[start] == '(')
            {
                var index = start;
                var line = engine.CurrentLine;
                var col = engine.CurrentColumn;
                var container = engine.Advance().ParseStatement(')', e => new YAMPBracketNotClosedError(line, col), (ch, statement) =>
                {
                    if (ch == ',')
                    {
                        var op = new CommaOperator(engine);
                        engine.Advance();
                        statement.Push(engine, op);
                        return true;
                    }

                    return false;
                }).Container;

                var exp = new BracketExpression(line, col, engine.Pointer - start, engine.Query, container ?? new ContainerExpression());

                if (container == null)
                    engine.AddError(new YAMPBracketEmptyError(line, col), exp);

                return exp;
            }

            return null;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            return "(" + base.ToCode() + ")";
        }

        #endregion
    }
}
