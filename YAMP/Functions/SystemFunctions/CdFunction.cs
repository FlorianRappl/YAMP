using System;
using System.IO;

namespace YAMP
{
    [Kind(PopularKinds.System)]
    [Description("Changes the current working directory.")]
    sealed class CdFunction : SystemFunction
    {
        [Description("The change directory method changes the currently set working directory. This enables you to access all files in the new directory with their file name only.")]
        [Example("cd(\"..\")", "Navigates to the upper directory of the current working directory.", true)]
        [Example("cd(\"C:/\")", "Navigates to the root directory of the drive C.", true)]
        [Example("cd(\"../../../\")", "Navigates three directories up.", true)]
        public void Function(StringValue path)
        {
            var p = path.Value;

            if (!Path.IsPathRooted(path.Value))
                p = Path.Combine(Environment.CurrentDirectory, path.Value);

            if (!Directory.Exists(path.Value))
                Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Failure, "The directory " + p + " could not be found."));
            else
                Environment.CurrentDirectory = path.Value;
        }
    }
}
