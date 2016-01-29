using System;
using System.Collections.Generic;

namespace YAMP.Help
{
    /// <summary>
    /// This is the entry for one object in the documentation.
    /// </summary>
	public class HelpSection
	{
        /// <summary>
        /// Gets or sets the name of the entry.
        /// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the topic for this entry.
        /// </summary>
		public string Topic { get; set; }

        /// <summary>
        /// Gets or sets the description of this entry.
        /// </summary>
		public string Description { get; set; }

        /// <summary>
        /// Gets or sets the hyperlink associated with this entry.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets the status if the entry has a webpage associated.
        /// </summary>
        public bool HasLink
        {
            get { return !string.IsNullOrEmpty(Link); }
        }
	}
}
