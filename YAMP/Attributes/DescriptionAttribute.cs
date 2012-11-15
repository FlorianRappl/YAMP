using System;

namespace YAMP
{
    /// <summary>
    /// Provides a description attribute to be read by the help method.
    /// </summary>
    public class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// Creates a new attribute for storing descriptions.
        /// </summary>
        /// <param name="description">The description to store.</param>
        public DescriptionAttribute(string description)
        {
            Description = description;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}
