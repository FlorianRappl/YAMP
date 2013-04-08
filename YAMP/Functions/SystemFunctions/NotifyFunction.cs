using System;

namespace YAMP
{
	[Description("Formats some text using placeholders like {0} etc. and notifies the user using the formatted string.")]
	[Kind(PopularKinds.System)]
    sealed class NotifyFunction : SystemFunction
	{
		[Description("Formats some text using placeholders like {0} etc. and notifies the user using the formatted string. This function does not return the string.")]
		[Example("notify(\"{2} The result of {0} + {1} is {3}\", 2.0, 7i, \"Hi!\", 2.0 + 7i)", "Evaluates the arguments and includes them in the string given by the first argument.")]
		[Arguments(1, 0)]
		public void Function(StringValue text, ArgumentsValue args)
		{
			var content = string.Format(text.Value, args.ToArray());
            Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Message, content));
		}
	}
}
