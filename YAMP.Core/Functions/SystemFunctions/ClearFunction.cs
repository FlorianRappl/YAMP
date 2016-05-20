namespace YAMP
{
    using System;

    [Description("ClearFunctionDescription")]
	[Kind(PopularKinds.System)]
    sealed class ClearFunction : SystemFunction
    {
        public ClearFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("ClearFunctionDescriptionForVoid")]
        [ExampleAttribute("clear()")]
        public void Function()
        {
            var count = 0;

            foreach (var name in Context.AllVariables.Keys)
            {
                Context.AssignVariable(name, null);
                count++;
            }

            Notify(count);
        }

        [Description("ClearFunctionDescriptionForArguments")]
        [ExampleAttribute("clear(\"x\")", "ClearFunctionExampleForArguments1")]
        [ExampleAttribute("clear(\"x\", \"y\", \"z\")", "ClearFunctionExampleForArguments2")]
		[Arguments(0, 1)]
        public void Function(ArgumentsValue args)
        {
            var count = 0;
            var allVariables = Context.AllVariables.Keys;

            foreach (var arg in args.Values)
            {
                if (arg is StringValue)
                {
                    var name = (arg as StringValue).Value;

                    if (allVariables.Contains(name))
                    {
                        Context.AssignVariable(name, null);
                        count++;
                    }
                }
            }

            Notify(count);
        }

        void Notify(Int32 count)
        {
            Context.RaiseNotification(new NotificationEventArgs(NotificationType.Information, count + " objects cleared."));
        }
    }
}
