using System;

namespace YAMP
{
	/// <summary>
	/// Provides a returns attribute to be read by the help method.
	/// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
	public class ReturnsAttribute : Attribute
	{
		/// <summary>
		/// Creates a new attribute for storing explanations for return values
		/// (should be used in combination with multiple output arguments).
        /// </summary>
        /// <param name="type">The type that will be returned</param>
		/// <param name="explanation">The specific explanations</param>
		public ReturnsAttribute(Type type, string explanation)
		{
            ReturnType = type;
			Explanation = explanation;
		}

		/// <summary>
		/// Gets the specified explanations for this return type.
		/// </summary>
		public string Explanation
		{
			get;
			private set;
		}

        /// <summary>
        /// Gets the type that will be returned.
        /// </summary>
        public Type ReturnType { get; private set; }
    }
}
