namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The function body missing.
    /// </summary>
    public class YAMPFunctionBodyMissing : YAMPParseError
    {
        internal YAMPFunctionBodyMissing(Int32 line, Int32 column)
            : base(line, column, "A function requires a body encapsulated in curly brackets.")
        {
        }

        internal YAMPFunctionBodyMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
