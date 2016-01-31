namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The scope not closed error.
    /// </summary>
    public class YAMPScopeNotClosedError : YAMPParseError
    {
        public YAMPScopeNotClosedError(Int32 line, Int32 column) :
            base(line, column, "The scope (curly bracket) starting at line {0}, column {1} has not been properly closed.", line, column)
        {
        }

        public YAMPScopeNotClosedError(ParseEngine pe) :
            this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
