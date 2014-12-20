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
using System.Reflection;

namespace YAMP
{
    /// <summary>
    /// Container for conserving information about parameters.
    /// </summary>
	public class FunctionParameters
    {
        #region ctor

        /// <summary>
        /// Creates a new instance of the function parameter holder.
        /// </summary>
        /// <param name="parameterInfo">The array of parameter infos.</param>
        /// <param name="methodInfo">The method info (from reflection).</param>
        public FunctionParameters(ParameterInfo[] parameterInfo, MethodInfo methodInfo)
		{
            int sum = 0;
            int minArg = parameterInfo.Length;
            int maxArg = parameterInfo.Length;
            var takeMax = false;

			Length = parameterInfo.Length;
			ParameterTypes = new Type[Length];

            for (var i = 0; i < Length; i++)
            {
                var t = parameterInfo[i].ParameterType;

				for (var j = 0; j < t.Name.Length; j++)
					sum += (int)t.Name[j];

                ParameterTypes[i] = t;
            }

            Weight = sum;
            OptionalArguments = methodInfo.GetCustomAttributes(typeof(ArgumentsAttribute), false) as ArgumentsAttribute[];

            foreach (var opt in OptionalArguments)
            {
                if (opt.MaximumArguments == int.MaxValue)
                    takeMax = true;
                else
                {
                    maxArg += opt.MaximumArguments;
                    maxArg--;
                }

                minArg--;
                minArg += opt.MinimumArguments;
            }

            MaximumArguments = takeMax ? int.MaxValue : maxArg;
            MinimumArguments = minArg;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets the computed optional arguments.
        /// </summary>
        public ArgumentsAttribute[] OptionalArguments
        {
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the specified parameter types.
        /// </summary>
		public Type[] ParameterTypes 
        {
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the length of the arguments.
        /// </summary>
        public int Length 
        {
            get;
            private set; 
        }

        /// <summary>
        /// Gets the computed weight of the arguments.
        /// </summary>
		public int Weight
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the minimum number of arguments for this function call.
        /// </summary>
		public int MinimumArguments
        {
            get;
            private set; 
		}

        /// <summary>
        /// Gets the maximum number of arguments for this function call.
        /// </summary>
		public int MaximumArguments
        {
            get;
            private set; 
		}

        #endregion
    }
}
