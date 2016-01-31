namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// A while argument is missing.
    /// </summary>
    public class YAMPWhileArgumentsMissing : YAMPParseError
    {
        internal YAMPWhileArgumentsMissing(Int32 line, Int32 column)
            : base(line, column, "The while keyword requires one (1) argument given in round brackets.")
        {
        }

        internal YAMPWhileArgumentsMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
