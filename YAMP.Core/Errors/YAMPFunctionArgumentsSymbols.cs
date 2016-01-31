namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The function arguments symbols errors.
    /// </summary>
    public class YAMPFunctionArgumentsSymbols : YAMPParseError
    {
        public YAMPFunctionArgumentsSymbols(Int32 line, Int32 column)
            : base(line, column, "A function requires a list of valid symbols as arguments. Only provide valid symbolnames as function arguments.")
        {
        }

        public YAMPFunctionArgumentsSymbols(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
