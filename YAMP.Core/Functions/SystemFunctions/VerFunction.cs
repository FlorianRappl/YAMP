using System;

namespace YAMP
{
    [Description("Returns the version of the YAMP parser engine.")]
    [Kind(PopularKinds.System)]
    sealed class VerFunction : SystemFunction
    {
        [Description("Gets a string containing the current version of the running YAMP engine.")]
        [Example("ver()", "Gets a string containing the version of YAMP.")]
        public StringValue Function()
        {
            return new StringValue(Parser.Version);
        }
    }
}
