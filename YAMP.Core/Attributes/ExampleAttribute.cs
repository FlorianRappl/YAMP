namespace YAMP
{
    using System;
    using System.ComponentModel;

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
        /// <param name="example">The example to store.</param>
        /// <param name="description">The description to store.</param>
        /// <param name="file">The status if the file system is manipulated.</param>
        public ExampleAttribute(string example, string description, bool file)
        {
            Example = example;
            Description = description;
            IsFile = file;
        }

        /// <summary>
        /// Creates a new example attribute with the specified example string.
        /// </summary>
        /// <param name="example">The example to store.</param>
        /// <param name="description">The description to store.</param>
        public ExampleAttribute(string example, string description)
            : this(example, description, false)
        {
        }

        /// <summary>
        /// Creates a new example attribute with the specified example string.
        /// </summary>
        /// <param name="example">The example to store.</param>
        public ExampleAttribute(string example) : this(example, string.Empty, false)
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
        public String Example { get; private set; }

        /// <summary>
        /// Gets the description of the example.
        /// </summary>
        public String Description { get; private set; }

        #endregion
    }
}
