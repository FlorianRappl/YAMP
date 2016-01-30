namespace YAMP
{
    /// <summary>
    /// SystemFunction is a special kind of ArgumentFunction, which saves the
    /// passed ParseContext in a variable, which can be accessed over the 
    /// property Context.
    /// </summary>
    public abstract class SystemFunction : ArgumentFunction
	{
		#region ctor

        /// <summary>
        /// Creates a new system function with a specific context.
        /// </summary>
        /// <param name="context">The given context.</param>
        public SystemFunction(ParseContext context)
        {
            Context = context;
        }

		#endregion

		#region Properties

        /// <summary>
        /// Gets or sets the associated context.
        /// </summary>
		public ParseContext Context 
        { 
            get; 
            private set; 
        }

		#endregion

		#region Methods

        /// <summary>
        /// Performs the function in the given context.
        /// </summary>
        /// <param name="context">The context where the function is executed.</param>
        /// <param name="argument">The argument of the function.</param>
        /// <returns>The evaluted value.</returns>
		public override Value Perform(ParseContext context, Value argument)
        {
			var function = GetType().GetConstructor(new [] { typeof(ParseContext) }).Invoke(new [] { context });
            return ((SystemFunction)function).Perform(argument);
		}

		#endregion
    }
}
