namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The function argument missing error.
    /// </summary>
    public class YAMPFunctionArgumentsMissing : YAMPParseError
    {
        internal YAMPFunctionArgumentsMissing(Int32 line, Int32 column)
            : base(line, column, "Arguments for the function have not been specified. Round brackets are always required.")
        {
        }

        internal YAMPFunctionArgumentsMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
