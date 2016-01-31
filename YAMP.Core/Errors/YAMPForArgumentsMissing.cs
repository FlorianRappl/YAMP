namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The for argument missing error.
    /// </summary>
    public class YAMPForArgumentsMissing : YAMPParseError
    {
        public YAMPForArgumentsMissing(Int32 line, Int32 column)
            : base(line, column, "The for keyword requires (3) arguments given in round brackets.")
        {
        }

        public YAMPForArgumentsMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
