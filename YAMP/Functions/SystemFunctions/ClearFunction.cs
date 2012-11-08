using System;

namespace YAMP
{
    [Description("Deletes variables from memory.")]
    class ClearFunction : SystemFunction
    {
        public ClearFunction() : base(1)
        {
        }

        [Description("Clears all variables.")]
        [ExampleAttribute("clear()")]
        public StringValue Function()
        {
            var count = 0;

            foreach (var name in Context.AllVariables.Keys)
            {
                Context.AssignVariable(name, null);
                count++;
            }

            return new StringValue(count + " objects cleared.");
        }

        [Description("Clears the specified variables given with their names as strings.")]
        [ExampleAttribute("clear(\"x\")", "Deletes the variable x.")]
        [ExampleAttribute("clear(\"x\", \"y\", \"z\")", "Deletes the variables x, y and z.")]
        public StringValue Function(ArgumentsValue args)
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

            return new StringValue(count + " objects cleared.");
        }
    }
}
