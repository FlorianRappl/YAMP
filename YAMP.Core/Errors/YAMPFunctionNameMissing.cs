namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The function name missing error.
    /// </summary>
    public class YAMPFunctionNameMissing : YAMPParseError
    {
        internal YAMPFunctionNameMissing(Int32 line, Int32 column)
            : base(line, column, "Functions without names are not allowed. Consider using lambda expressions for such scenarios.")
        {
        }

        internal YAMPFunctionNameMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
