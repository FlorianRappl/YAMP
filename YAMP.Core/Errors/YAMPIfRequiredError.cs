namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The if required error.
    /// </summary>
	public class YAMPIfRequiredError : YAMPParseError
	{
		public YAMPIfRequiredError(Int32 line, Int32 column) 
            : base(line, column, "An else block requires an if statement.")
		{
		}

        public YAMPIfRequiredError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
	}
}