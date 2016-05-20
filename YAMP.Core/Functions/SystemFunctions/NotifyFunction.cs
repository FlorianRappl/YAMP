namespace YAMP
{
    using System;

	[Description("NotifyFunctionDescription")]
	[Kind(PopularKinds.System)]
    sealed class NotifyFunction : SystemFunction
	{
        public NotifyFunction(ParseContext context)
            : base(context)
        {
        }

		[Description("NotifyFunctionDescriptionForStringArguments")]
		[Example("notify(\"{2} The result of {0} + {1} is {3}\", 2.0, 7i, \"Hi!\", 2.0 + 7i)", "NotifyFunctionExampleForStringArguments1")]
		[Arguments(1, 0)]
		public void Function(StringValue text, ArgumentsValue args)
		{
			var content = String.Format(text.Value, args.ToArray());
            Context.RaiseNotification(new NotificationEventArgs(NotificationType.Message, content));
		}
	}
}
