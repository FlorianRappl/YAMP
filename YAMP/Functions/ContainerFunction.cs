using System;

namespace YAMP
{
    [Description("A custom function defined by you.")]
    class ContainerFunction : IFunction
    {
        string name;
        FunctionDelegate execution;

        public ContainerFunction(string name, FunctionDelegate execution)
        {
            this.name = name;
            this.execution = execution;
        }

        public string Name
        {
            get { return name; }
        }

        [Description("Executes the custom function with your code.")]
        [Example("-", "No help available.")]
        public Value Perform(ParseContext context, Value argument)
        {
            return execution(argument);
        }
    }
}
