using System;

namespace YAMP.Help
{
    /// <summary>
    /// Represents an entry in the documentation.
    /// </summary>
	public class HelpEntry
	{
        /// <summary>
        /// Gets or sets the name of the function.
        /// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Gets or sets an instance of the corresponding function.
        /// </summary>
		public object Instance { get; set; }

        /// <summary>
        /// Gets or sets the associated help topic.
        /// </summary>
		public HelpTopic Topic { get; set; }
	}
}
