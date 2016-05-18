namespace YAMP
{
    using System;

    /// <summary>
    /// Class to enter examples for usage with help.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ExampleAttribute : Attribute
    {
        #region ctor

        /// <summary>
        /// Creates a new example attribute with the specified example string.
        /// </summary>
        /// <param name="exampleCode">The example to store.</param>
        /// <param name="descriptionKey">The description to store.</param>
        /// <param name="file">The status if the file system is manipulated.</param>
        public ExampleAttribute(String exampleCode, String descriptionKey, Boolean file)
        {
            ExampleCode = exampleCode;
            DescriptionKey = descriptionKey;
            IsFile = file;
        }

        /// <summary>
        /// Creates a new example attribute with the specified example string.
        /// </summary>
        /// <param name="exampleCode">The example to store.</param>
        /// <param name="descriptionKey">The description to store.</param>
        public ExampleAttribute(String exampleCode, String descriptionKey)
            : this(exampleCode, descriptionKey, false)
        {
        }

        /// <summary>
        /// Creates a new example attribute with the specified example string.
        /// </summary>
        /// <param name="exampleCode">The example to store.</param>
        public ExampleAttribute(String exampleCode) 
            : this(exampleCode, String.Empty, false)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the example performs an operation on the file system.
        /// </summary>
        public Boolean IsFile { get; private set; }

        /// <summary>
        /// Gets the example.
        /// </summary>
        public String ExampleCode { get; private set; }

        /// <summary>
        /// Gets the description of the example.
        /// </summary>
        public String DescriptionKey { get; private set; }

        #endregion
    }
}
