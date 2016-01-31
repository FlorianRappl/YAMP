namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The escape sequence not found error.
    /// </summary>
	public class YAMPEscapeSequenceNotFoundError : YAMPParseError
	{
        internal YAMPEscapeSequenceNotFoundError(Int32 line, Int32 column, Char sequence) 
            : base(line, column, "The escape sequence \\{0} is not recognized.", sequence)
		{
		}

        internal YAMPEscapeSequenceNotFoundError(ParseEngine pe, Char sequence)
            : this(pe.CurrentLine, pe.CurrentColumn, sequence)
        {
        }
	}
}
