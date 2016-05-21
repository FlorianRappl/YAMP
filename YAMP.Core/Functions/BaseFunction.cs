namespace YAMP
{
    using System;
    using YAMP.Numerics;

	/// <summary>
	/// The abstract base class for StandardFunctions and ArgumentFunctions
	/// </summary>
	public abstract class BaseFunction : IFunction
    {
        #region Fields

        internal static readonly Generator Rng = new MT19937Generator();
        readonly String _name;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a function class with the name chosen by convention.
        /// </summary>
        public BaseFunction()
        {
            _name = GetType().Name.RemoveFunctionConvention().ToLower();
        }

        /// <summary>
        /// Creates a function class.
        /// </summary>
        /// <param name="name">The name for the function.</param>
        public BaseFunction(String name)
        {
            _name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the function.
        /// </summary>
        /// <param name="argument">The function's argument(s).</param>
        /// <returns>The result.</returns>
        public abstract Value Perform(Value argument);

        /// <summary>
        /// Invokes the function.
        /// </summary>
        /// <param name="context">The context of the invocation.</param>
        /// <param name="argument">The function's argument(s).</param>
        /// <returns>The result.</returns>
		public virtual Value Perform(ParseContext context, Value argument)
		{
			return Perform(argument);
        }

        #endregion
    }
}
