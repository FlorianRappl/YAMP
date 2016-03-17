namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Errors;

    /// <summary>
    /// The absolute expression |...| which returns the absolute value
    /// of the evaluated value inside.
    /// </summary>
    class AbsExpression : TreeExpression
    {
        #region ctor

        public AbsExpression()
        {
        }

        public AbsExpression(Int32 line, Int32 column, Int32 length, QueryContext query, ContainerExpression child)
            : base(child, query, line, column)
        {
            Length = length;
        }

        #endregion

        #region Methods

        public override Value Interpret(IDictionary<String, Value> symbols)
        {
            return AbsFunction.Abs(base.Interpret(symbols));
        }

        public override Expression Scan(ParseEngine engine)
        {
            var chars = engine.Characters;
            var start = engine.Pointer;

            if (chars[start] == '|')
            {
                var line = engine.CurrentLine;
                var col = engine.CurrentColumn;
                var container = engine.Advance().ParseStatement('|', e => new YAMPTerminatorMissingError(line, col, '|')).Container;
                return new AbsExpression(line, col, engine.Pointer - start, engine.Query, container);
            }

            return null;
        }

        #endregion

        #region String Representations

        public override String ToCode()
        {
            return "|" + base.ToCode() + "|";
        }

        #endregion
    }
}