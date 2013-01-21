using System;

namespace YAMP
{
	[Description("Formats some text using placeholders like {0} etc. and prints the resulting string on the sreen.")]
	[Kind(PopularKinds.System)]
	class PrintfFunction : SystemFunction
	{
		[Description("Formats some text using placeholders like {0} etc. and prints the resulting string. This function does not return the string.")]
		[Example("printf(\"{2} The result of {0} + {1} is {3}\", 2.0, 7i, \"Hi!\", 2.0 + 7i)", "Evaluates the arguments and includes them in the string given by the first argument. Prints them on the screen directly.")]
		[Arguments(1, 0)]
		public void Function(StringValue text, ArgumentsValue args)
		{
			var content = string.Format(text.Value, args.ToArray());
            Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Message, content));
		}
	}
}
