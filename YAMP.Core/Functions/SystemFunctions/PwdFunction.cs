namespace YAMP
{
    using System;

    [Kind(PopularKinds.System)]
	[Description("Outputs the currently set working directory. This directory is used for all relative paths.")]
    sealed class PwdFunction : SystemFunction
	{
        public PwdFunction(ParseContext context)
            : base(context)
        {
        }

		[Description("Prints the working directory.")]
		[Example("pwd()", "Outputs the current directory.")]
		public StringValue Function()
		{
			return new StringValue(Environment.CurrentDirectory);
		}
	}
}
