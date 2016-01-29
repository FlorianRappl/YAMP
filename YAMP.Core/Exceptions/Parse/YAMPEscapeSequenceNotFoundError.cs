using System;

namespace YAMP
{
	class YAMPEscapeSequenceNotFoundError : YAMPParseError
	{
		public YAMPEscapeSequenceNotFoundError(int line, int column, char sequence) 
            : base(line, column, "The escape sequence \\{0} is not recognized.", sequence)
		{
		}

        public YAMPEscapeSequenceNotFoundError(ParseEngine pe, char sequence)
            : this(pe.CurrentLine, pe.CurrentColumn, sequence)
        {
        }
	}
}
