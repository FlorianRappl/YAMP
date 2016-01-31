namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The expression missing error.
    /// </summary>
    public class YAMPExpressionMissingError : YAMPParseError
    {
        internal YAMPExpressionMissingError(Int32 line, Int32 column)
            : base(line, column, "Another expression was expected but nothing was found.")
        {
        }

        internal YAMPExpressionMissingError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }

        internal YAMPExpressionMissingError(ParseEngine pe, Operator op, Int32 found)
            : base(pe.CurrentLine, pe.CurrentColumn, "The {0} operator requires {1} expression(s), but only {2} expression(s) have been found.", op.Op, op.Expressions, found)
        {
        }
    }
}
