namespace YAMP.Help
{
    using System;

    /// <summary>
    /// Represents an entry in the documentation.
    /// </summary>
	public class HelpEntry
	{
        /// <summary>
        /// Gets or sets the name of the function.
        /// </summary>
		public String Name { get; set; }

        /// <summary>
        /// Gets or sets an instance of the corresponding function.
        /// </summary>
		public Object Instance { get; set; }

        /// <summary>
        /// Gets or sets the associated help topic.
        /// </summary>
		public HelpTopic Topic { get; set; }
	}
}
