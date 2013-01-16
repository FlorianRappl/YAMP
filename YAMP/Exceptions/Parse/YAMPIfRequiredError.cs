using System;

namespace YAMP
{
	class YAMPIfRequiredError : YAMPParseError
	{
		public YAMPIfRequiredError(int line, int column) 
            : base(line, column, "An else block requires an if statement.")
		{
		}

        public YAMPIfRequiredError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
	}
}