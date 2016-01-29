namespace YAMP.Help
{
    using System;

    /// <summary>
    /// This is an example entry of a function help entry.
    /// </summary>
	public class HelpExample
	{
        /// <summary>
        /// Gets or sets the specified example.
        /// </summary>
		public string Example { get; set; }

        /// <summary>
        /// Gets or sets the given description of the example.
        /// </summary>
		public string Description { get; set; }

        /// <summary>
        /// Gets or sets if the example accesses the file system.
        /// </summary>
        public bool IsFile { get; set; }
	}
}
