/*
	Copyright (c) 2012-2014, Florian Rappl.
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
		  notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright
		  notice, this list of conditions and the following disclaimer in the
		  documentation and/or other materials provided with the distribution.
		* Neither the name of the YAMP team nor the names of its contributors
		  may be used to endorse or promote products derived from this
		  software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

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
