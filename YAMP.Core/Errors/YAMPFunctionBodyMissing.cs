namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The function body missing.
    /// </summary>
    public class YAMPFunctionBodyMissing : YAMPParseError
    {
        public YAMPFunctionBodyMissing(Int32 line, Int32 column)
            : base(line, column, "A function requires a body encapsulated in curly brackets.")
        {
        }

        public YAMPFunctionBodyMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
