using System;
using System.IO;

namespace YAMP
{
	[Kind(PopularKinds.System)]
	[Description("Copies a source file to the specified target.")]
	class CpFunction : SystemFunction
	{
		[Description("Copies one file from a source location to some target location.")]
		[Example("cp(\"myfile.data\", \"evaluation.txt\")", "Copies the file myfile.data to evaluation.txt. The file myfile.data must be available in the current working directory.")]
		[Example("cp(\"data/some.data\", \".\")", "Copies the file some.data of the directory data to the current directory without renaming the file.")]
		public StringValue Function(StringValue source, StringValue target)
		{
			File.Copy(source.Value, target.Value);
			return new StringValue(string.Format("Copied {0} to {1}.", source.Value, target.Value));
		}
	}
}
