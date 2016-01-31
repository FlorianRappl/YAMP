namespace YAMP.Io
{
    using System;
    using System.IO;

	[Kind(PopularKinds.System)]
	[Description("Moves a source file to a new target and / or renames the file.")]
    sealed class MvFunction : SystemFunction
	{
        public MvFunction(ParseContext context)
            : base(context)
        {
        }

		[Description("Moves one file from a source location to a new target location.")]
		[Example("mv(\"myfile.data\", \"evaluation.txt\")", "Renames the file myfile.data to evaluation.txt. The file must be available in the current working directory.", true)]
		[Example("mv(\"data/some.data\", \".\")", "Moves the file some.data of the directory data to the current directory without renaming the file.", true)]
		public void Function(StringValue source, StringValue target)
		{
			File.Move(source.Value, target.Value);
            RaiseNotification(NotificationType.Success, String.Format("Moved {0} to {1}.", source.Value, target.Value));
		}
	}
}
