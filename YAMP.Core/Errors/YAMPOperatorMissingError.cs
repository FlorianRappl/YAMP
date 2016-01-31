namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The operator missing error.
    /// </summary>
    class YAMPOperatorMissingError : YAMPParseError
    {
        public YAMPOperatorMissingError(Int32 line, Int32 column)
            : base(line, column, "An expected operator was not found.")
        {
        }

        public YAMPOperatorMissingError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
