using System.Collections.Generic;

namespace YAMP
{
    [Description("Dummy class; does nothing.")]
    [Kind(PopularKinds.System)]
    class SaveFunction : SystemFunction
	{
        [Description("Dummy method; does nothing.")]
        public static void Save(string filename, IDictionary<string, Value> workspace)
		{
        }
	}
}

