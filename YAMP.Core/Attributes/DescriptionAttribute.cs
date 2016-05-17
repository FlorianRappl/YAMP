namespace YAMP
{
    using System;

    /// <summary>
    /// Provides a description attribute to be read by the help method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class DescriptionAttribute : Attribute
    {
        #region ctor

        /// <summary>
        /// Creates a new attribute for storing descriptions.
        /// </summary>
        /// <param name="description">The description to store.</param>
        public DescriptionAttribute(string description)
        {
            Description = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the description.
        /// </summary>
        public String Description { get; private set; }

        #endregion
    }
}
