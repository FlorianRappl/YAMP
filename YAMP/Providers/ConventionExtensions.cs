using System;

namespace YAMP
{
    /// <summary>
    /// Contains some extensions used to tackle some conventions used in the code.
    /// </summary>
    public static class ConventionExtensions
    {
        public static string RemoveFunctionConvention(this string functionName)
        {
            return functionName.Replace("Function", string.Empty);
        }

        public static string RemoveValueConvention(this string valueName)
        {
            if (valueName.Equals("Value"))
                return valueName;

            return valueName.Replace("Value", string.Empty);
        }

        public static bool IsArgumentFunction(this string functionName)
        {
            return functionName.Equals("Function");
        }
    }
}
