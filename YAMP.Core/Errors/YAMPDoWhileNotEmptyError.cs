namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// A do while statement is wrong.
    /// </summary>
    public class YAMPDoWhileNotEmptyError : YAMPParseError
    {
        public YAMPDoWhileNotEmptyError(Int32 line, Int32 column)
            : base(line, column, "A do-while construct requires an empty statement after the while condition. Consider inserting a semicolon after the condition.")
        {
        }

        public YAMPDoWhileNotEmptyError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
