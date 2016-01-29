using System;

namespace YAMP
{
    [Kind(PopularKinds.System)]
	[Description("Outputs the currently set working directory. This directory is used for all relative paths.")]
    sealed class PwdFunction : SystemFunction
	{
		[Description("Prints the working directory.")]
		[Example("pwd()", "Outputs the current directory.")]
		public StringValue Function()
		{
			return new StringValue(Environment.CurrentDirectory);
		}
	}
}
