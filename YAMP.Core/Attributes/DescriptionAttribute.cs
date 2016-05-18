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
        /// <param name="descriptionKey">The description to store.</param>
        public DescriptionAttribute(String descriptionKey)
        {
            DescriptionKey = descriptionKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the description.
        /// </summary>
        public String DescriptionKey { get; private set; }

        #endregion
    }
}
