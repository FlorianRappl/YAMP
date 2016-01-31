namespace YAMP.Help
{
    using System.Collections.Generic;

    /// <summary>
    /// This is the extended version of the help section class (entry of an
    /// object) - specialized for functions.
    /// </summary>
	public class HelpFunctionSection : HelpSection
	{
        /// <summary>
        /// Creates a new instance.
        /// </summary>
		public HelpFunctionSection()
		{
			Usages = new List<HelpFunctionUsage>();
		}

        /// <summary>
        /// Gets the available usages of the function.
        /// </summary>
		public List<HelpFunctionUsage> Usages { get; private set; }
	}
}
