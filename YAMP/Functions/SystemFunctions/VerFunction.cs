using System;

namespace YAMP
{
    [Description("Returns the version of the YAMP parser engine.")]
    [Kind(PopularKinds.System)]
    class VerFunction : SystemFunction
    {
        [Description("Gets a string containing the current version of the running YAMP engine.")]
        public StringValue Function()
        {
            return new StringValue(Parser.Version);
        }
    }
}
