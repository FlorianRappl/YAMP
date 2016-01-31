namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The matrix not closed error.
    /// </summary>
    public class YAMPMatrixNotClosedError : YAMPParseError
    {
        internal YAMPMatrixNotClosedError(Int32 line, Int32 column) :
            base(line, column, "The matrix (square bracket) starting at line {0}, column {1} has not been properly closed.", line, column)
        {
        }

        internal YAMPMatrixNotClosedError(ParseEngine pe) :
            this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
