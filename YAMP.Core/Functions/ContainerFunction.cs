namespace YAMP
{
    using System;

    /// <summary>
    /// This class is used as a wrapper for functions just defined by passing
    /// a delegate.
    /// </summary>
    [Description("ContainerFunctionDescription")]
	[Kind(PopularKinds.Function)]
    class ContainerFunction : IFunction
    {
        #region Fields

        readonly String _name;
        readonly FunctionDelegate _execution;

        #endregion

        #region ctor

        public ContainerFunction(String name, FunctionDelegate execution)
        {
            _name = name;
            _execution = execution;
        }

        #endregion

        #region Properties

        public String Name
        {
            get { return _name; }
        }

        #endregion

        #region Methods

        [Description("ContainerFunctionDescriptionForContextValue")]
        [Example("-", "NoHelp")]
        public Value Perform(ParseContext context, Value argument)
        {
            return _execution(argument);
        }

        #endregion
    }
}
