using System;
using System.Collections.Generic;

namespace YAMP.Help
{
	public class HelpTopic : List<HelpEntry>
	{
		public HelpTopic(string kind)
		{
			Kind = kind;
		}

		public string Kind { get; private set; }
	}
}
