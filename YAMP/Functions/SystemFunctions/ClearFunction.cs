using System;

namespace YAMP
{
	[Description("Deletes variables from memory.")]
	[Kind(PopularKinds.System)]
    sealed class ClearFunction : SystemFunction
    {
        [Description("Clears all variables.")]
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

        [Description("Clears the specified variables given with their names as strings.")]
        [ExampleAttribute("clear(\"x\")", "Deletes the variable x.")]
        [ExampleAttribute("clear(\"x\", \"y\", \"z\")", "Deletes the variables x, y and z.")]
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

        void Notify(int count)
        {
            Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Information, count + " objects cleared."));
        }
    }
}
