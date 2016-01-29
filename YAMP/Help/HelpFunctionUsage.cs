namespace YAMP.Help
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents one usage of the described function.
    /// </summary>
	public class HelpFunctionUsage
	{
        /// <summary>
        /// Creates a new instance.
        /// </summary>
		public HelpFunctionUsage()
		{
			Examples = new List<HelpExample>();
			Arguments = new List<string>();
            ArgumentNames = new List<string>();
            Returns = new List<string>();
		}

        /// <summary>
        /// Gets or sets the usage.
        /// </summary>
		public string Usage { get; set; }

        /// <summary>
        /// Gets or sets a description about the usage.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets a list of names for the given arguments of this function usage.
        /// </summary>
        public List<string> ArgumentNames { get; private set; }

        /// <summary>
        /// Gets a list of arguments for this function usage.
        /// </summary>
		public List<string> Arguments { get; private set; }

        /// <summary>
        /// Gets a list of available return values of this function usage.
        /// </summary>
        public List<string> Returns { get; set; }

        /// <summary>
        /// Gets a list of examples corresponding to this function usage.
        /// </summary>
		public List<HelpExample> Examples { get; private set; }
	}
}
