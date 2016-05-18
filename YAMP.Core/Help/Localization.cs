namespace YAMP.Help
{
    using System;
    using System.Collections.Generic;

    public static class Localization
    {
        public static readonly Dictionary<String, String> Default = new Dictionary<String, String>
        {
            { "NoDescription", "No description available." }
        };

        public static Dictionary<String, String> Current = Default;
    }
}
