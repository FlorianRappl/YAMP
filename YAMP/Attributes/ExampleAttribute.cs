using System;
using System.ComponentModel;

namespace YAMP
{
    /// <summary>
    /// Class to enter examples for usage with help.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ExampleAttribute : Attribute
    {
        /// <summary>
        /// Creates a new example attribute with the specified example string.
        /// </summary>
        /// <param name="example">The example to store.</param>
        /// <param name="description">The description to store.</param>
        public ExampleAttribute(string example, string description)
        {
            Example = example;
            Description = description;
        }

        /// <summary>
        /// Creates a new example attribute with the specified example string.
        /// </summary>
        /// <param name="example">The example to store.</param>
        public ExampleAttribute(string example) : this(example, string.Empty)
        {
        }

        /// <summary>
        /// Gets the example.
        /// </summary>
        public string Example { get; private set; }

        /// <summary>
        /// Gets the description of the example.
        /// </summary>
        public string Description { get; private set; }
    }
}
