namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The if argument missing error.
    /// </summary>
    public class YAMPIfArgumentsMissing : YAMPParseError
    {
        internal YAMPIfArgumentsMissing(Int32 line, Int32 column)
            : base(line, column, "The if keyword requires one (1) argument given in round brackets.")
        {
        }

        internal YAMPIfArgumentsMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
