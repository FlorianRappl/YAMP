using System;

namespace YAMP
{
	/// <summary>
	/// Provides a returns attribute to be read by the help method.
	/// </summary>
	public class ReturnsAttribute : Attribute
	{
		/// <summary>
		/// Creates a new attribute for storing explanations for return values
		/// (should be used in combination with multiple output arguments).
		/// </summary>
		/// <param name="explanation">The specific explanations</param>
		public ReturnsAttribute(params string[] explanation)
		{
			ReturnArguments = explanation;
		}

		/// <summary>
		/// Gets the specified explanations for the return arguments.
		/// </summary>
		public string[] ReturnArguments
		{
			get;
			private set;
		}
	}
}
