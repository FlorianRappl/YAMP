using System;

namespace YAMP
{
    /// <summary>
    /// This class is used as a wrapper for functions just defined by passing
    /// a delegate.
    /// </summary>
    [Description("A custom function defined by you.")]
	[Kind(PopularKinds.Function)]
    class ContainerFunction : IFunction
    {
        #region Fields

        string name;
        FunctionDelegate execution;

        #endregion

        #region ctor

        public ContainerFunction(string name, FunctionDelegate execution)
        {
            this.name = name;
            this.execution = execution;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
        }

        #endregion

        #region Methods

        [Description("Executes the custom function with your code.")]
        [Example("-", "No help available.")]
        public Value Perform(ParseContext context, Value argument)
        {
            return execution(argument);
        }

        #endregion
    }
}
