using System;

namespace YAMP
{
	/// <summary>
	/// The abstract base class for StandardFunctions and ArgumentFunctions
	/// </summary>
	public abstract class BaseFunction : IFunction
    {
        #region Fields

        string name;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a function class with the name chosen by convention.
        /// </summary>
        public BaseFunction()
        {
            name = GetType().Name.RemoveFunctionConvention().ToLower();
        }

        /// <summary>
        /// Creates a function class.
        /// </summary>
        /// <param name="name">The name for the function.</param>
        public BaseFunction(string name)
        {
            this.name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
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
