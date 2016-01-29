using System;

namespace YAMP
{
	/// <summary>
	/// Provides a kind attribute to be read by the help method. This attribute specifies the kind of function / constant that is declared.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
	public class KindAttribute : Attribute
    {
        #region ctor

        /// <summary>
        /// Creates a new attribute for storing the kind of a function.
        /// </summary>
		/// <param name="kind">The kind to store.</param>
		public KindAttribute(string kind)
        {
			Kind = kind;
        }

		/// <summary>
		/// Creates a new attribute for storing the kind of a function.
		/// </summary>
		/// <param name="kind">The kind to store.</param>
		public KindAttribute(PopularKinds kind) : this(kind.ToString())
		{
		}

        #endregion

        #region Property

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public string Kind { get; private set; }

        #endregion
    }
}
