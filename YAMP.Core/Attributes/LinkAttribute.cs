namespace YAMP
{
    using System;

    /// <summary>
    /// Provides a kind attribute to be read by the help method. This attribute specifies the kind of function / constant that is declared.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LinkAttribute : Attribute
    {
        #region ctor

        /// <summary>
        /// Creates a new attribute for storing more information about a function.
        /// </summary>
        /// <param name="urlKey">The url to store.</param>
        public LinkAttribute(String urlKey)
        {
            UrlKey = urlKey;
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets the stored URL.
        /// </summary>
        public String UrlKey { get; private set; }

        #endregion
    }
}
