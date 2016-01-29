using System;

namespace YAMP
{
    /// <summary>
    /// Contains some extensions used to tackle some conventions used in the code.
    /// </summary>
    public static class ConventionExtensions
    {
        /// <summary>
        /// Removes the function convention from a string.
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns>The string without the word Function.</returns>
        public static string RemoveFunctionConvention(this string functionName)
        {
            return functionName.Replace("Function", string.Empty);
        }

        /// <summary>
        /// Removes the value convention from a string.
        /// </summary>
        /// <param name="valueName"></param>
        /// <returns>The string without the word Value.</returns>
        public static string RemoveValueConvention(this string valueName)
        {
            if (valueName.Equals("Value"))
                return valueName;

            return valueName.Replace("Value", string.Empty);
        }

        /// <summary>
        /// Detects if the given function name belongs to an argument function.
        /// </summary>
        /// <param name="functionName">The given function name.</param>
        /// <returns>True if the name is equal to Function, otherwise false.</returns>
        public static bool IsArgumentFunction(this string functionName)
        {
            return functionName.Equals("Function");
        }

        /// <summary>
        /// Removes the expression convention from a string.
        /// </summary>
        /// <param name="expressionName"></param>
        /// <returns>The string without the word Expression.</returns>
        public static string RemoveExpressionConvention(this string expressionName)
        {
            return expressionName.Replace("Expression", string.Empty);
        }

        /// <summary>
        /// Removes the operator convention from a string.
        /// </summary>
        /// <param name="operatorName"></param>
        /// <returns>The string without the word Operator.</returns>
        public static string RemoveOperatorConvention(this string operatorName)
        {
            return operatorName.Replace("Operator", string.Empty);
        }
    }
}
