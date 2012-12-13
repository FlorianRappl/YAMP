using System;
using System.Collections.Generic;

namespace YAMP.Help
{
	public class HelpFunctionUsage
	{
		public HelpFunctionUsage()
		{
			Examples = new List<HelpExample>();
			Arguments = new List<string>();
            ArgumentNames = new List<string>();
            Returns = new List<string>();
		}

		public string Usage { get; set; }

        public string Description { get; set; }

        public List<string> ArgumentNames { get; private set; }

		public List<string> Arguments { get; private set; }

        public List<string> Returns { get; set; }

		public List<HelpExample> Examples { get; private set; }
	}
}
