namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The expression expected error.
    /// </summary>
    public class YAMPExpressionExpectedError : YAMPParseError
    {
        public YAMPExpressionExpectedError(Int32 line, Int32 column)
            : base(line, column, "An expression was expected but an operator was found.")
        {
        }

        public YAMPExpressionExpectedError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
