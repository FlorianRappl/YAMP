using System;
using System.Collections.Generic;

namespace YAMP.Help
{
    /// <summary>
    /// This is the help topic, i.e. the list with help entries for a certain kind of
    /// function.
    /// </summary>
	public class HelpTopic : List<HelpEntry>
	{
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="kind">The kind of topic.</param>
		public HelpTopic(string kind)
		{
			Kind = kind;
		}

        /// <summary>
        /// Gets the kind represented by this topic.
        /// </summary>
		public string Kind { get; private set; }
	}
}
