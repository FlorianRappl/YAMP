using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace YAMP
{
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

        public AbsExpression(int line, int column, int length, QueryContext query, ContainerExpression child)
            : base(child, query, line, column)
        {
            Length = length;
        }

        #endregion

        #region Methods

        public override Value Interpret(Dictionary<string, Value> symbols)
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

        public override string ToCode()
        {
            return "|" + base.ToCode() + "|";
        }

        #endregion
    }
}