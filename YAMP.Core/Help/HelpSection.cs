namespace YAMP.Help
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the entry for one object in the documentation.
    /// </summary>
	public class HelpSection
	{
        /// <summary>
        /// Gets or sets the name of the entry.
        /// </summary>
		public String Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the topic for this entry.
        /// </summary>
		public String Topic { get; set; }

        /// <summary>
        /// Gets or sets the description of this entry.
        /// </summary>
		public String Description { get; set; }

        /// <summary>
        /// Gets or sets the hyperlink associated with this entry.
        /// </summary>
        public String Link { get; set; }

        /// <summary>
        /// Gets the status if the entry has a webpage associated.
        /// </summary>
        public Boolean HasLink
        {
            get { return !String.IsNullOrEmpty(Link); }
        }
	}
}
