namespace YAMP
{
    using System;

    /// <summary>
    /// The abstract base class used for all MemberFunction functions (with @this).
    /// </summary>
	public abstract class MemberFunction : ArgumentFunction
	{
        #region Fields

        //It doesn't need to be ThreadStatic. It's up to the YAMP runtime to make sure there is a specific instance of the MemberFunction during each call.
        //If it's assured by the YAMP runtime that (even in the event of multiple *same* functions) they are being evaluated by order, and one at a time,
        //it could be replaced with a static variable, BUT THEN, make it ThreadStatic in case the engine is called on multiple threads simultaneously:
        //[ThreadStatic]
        //private static Value _this = Value.Empty;

        private Value _this = Value.Empty;

        #endregion

        #region Properties
        //The state must be independent on any MemberFunction instances, in case there are multiple (same) functions on the same scope being evaluated...
        //So, the runtime must create a unique instance when needed. Don't just use the reference-copy from the RootContext/Function's dictionary.
        //To be safe, don't even set any object, if the function won't be called on the scope of an object. (like a "static" function).

        /// <summary>
        /// The "this" object of the function call
        /// </summary>
        public Value @this
        {
            private set { }
            get { return _this; }
        }

        /// <summary>
        /// Performs the function execution.
        /// </summary>
        /// <param name="argument">The argument(s) (if presented in an "ArgumentValue").</param>
        /// <returns>The evaluated value.</returns>
        public override Value Perform(Value argument)
        {
            return base.Perform(argument);
        }

        public MemberFunction CreateMemberFunctionInstance(Value state)
        {
            var clone = this.MemberwiseClone() as MemberFunction;
            clone._this = state;

            return clone;
        }


        #endregion
    }
}

